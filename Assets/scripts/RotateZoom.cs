using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInteractionWithConstraints : MonoBehaviour
{
    public Transform targetObject;      
    public float rotationSpeed = 0.2f; 
    public float zoomSpeed = 0.1f;     
    public float panSpeed = 0.002f;  // Reduced pan speed to avoid large movement
    public float minScale = 0.5f;      
    public float maxScale = 3f;        
    public float minX = -1f;           
    public float maxX = 1f;            
    public float minZ = -1f;           
    public float maxZ = 1f;            
    public float zoomSmoothing = 8f;   // Increased smoothing for better effect

    private Vector2 lastTouchPosition; 
    private float initialDistance;     
    private Vector3 initialScale;      

    void Update()
    {
        if (Input.touchCount == 1)
        {
            HandleRotation();
        }
        else if (Input.touchCount == 2)
        {
            HandleZoomAndPan();
        }
    }

    // One-finger rotation
    private void HandleRotation()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            lastTouchPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = touch.position - lastTouchPosition;

            float rotationY = -touchDelta.x * rotationSpeed; 
            float rotationX = touchDelta.y * rotationSpeed;  

            targetObject.Rotate(Vector3.up, rotationY, Space.World);   
            targetObject.Rotate(Vector3.right, rotationX, Space.Self); 

            lastTouchPosition = touch.position;
        }
    }

    // Two-finger zoom and pan
    private void HandleZoomAndPan()
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        float currentDistance = Vector2.Distance(touch0.position, touch1.position);

        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            initialDistance = currentDistance;
            initialScale = targetObject.localScale;
        }
        else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
        {
            // Zooming
            float scaleFactor = currentDistance / initialDistance;
            Vector3 targetScale = initialScale * scaleFactor;
            targetScale = ClampScale(targetScale, minScale, maxScale);

            // Smooth zooming
            targetObject.localScale = Vector3.Lerp(targetObject.localScale, targetScale, Time.deltaTime * zoomSmoothing);

            // Pan (Move only if zoom is not too fast)
            Vector2 touchDelta0 = touch0.deltaPosition;
            Vector2 touchDelta1 = touch1.deltaPosition;
            Vector2 averageDelta = (touchDelta0 + touchDelta1) / 2;

            // Reduce pan speed based on zoom level
            float adjustedPanSpeed = panSpeed / targetObject.localScale.magnitude;

            Vector3 panTranslation = new Vector3(averageDelta.x * adjustedPanSpeed, 0, averageDelta.y * adjustedPanSpeed);
            targetObject.Translate(panTranslation, Space.World);

            // Clamp position within defined limits
            Vector3 clampedPosition = new Vector3(
                Mathf.Clamp(targetObject.position.x, minX, maxX),
                targetObject.position.y,
                Mathf.Clamp(targetObject.position.z, minZ, maxZ)
            );
            targetObject.position = clampedPosition;
        }
    }

    // Clamp scaling to prevent extreme sizes
    private Vector3 ClampScale(Vector3 scale, float min, float max)
    {
        return new Vector3(
            Mathf.Clamp(scale.x, min, max),
            Mathf.Clamp(scale.y, min, max),
            Mathf.Clamp(scale.z, min, max)
        );
    }
}
