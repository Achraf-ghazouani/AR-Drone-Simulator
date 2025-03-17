using UnityEngine;

public class DroneCameraFollow : MonoBehaviour
{
    public Transform drone; // Reference to the drone's transform
    public Vector3 offset = new Vector3(0, 3, -5); // Offset from the drone
    public float smoothSpeed = 0.3f; // Increased smooth speed for faster response
    public float followSpeed = 5f; // Speed of following the drone movement

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (drone == null)
            return;
        
        // Desired position of the camera
        Vector3 targetPosition = drone.position + drone.TransformDirection(offset);
        
        // Move the camera to follow the drone smoothly
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Make the camera always look at the drone
        transform.LookAt(drone.position);
    }
}