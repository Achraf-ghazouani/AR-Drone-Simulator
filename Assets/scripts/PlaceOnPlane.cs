using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    public GameObject objectToPlace; 
    public GameObject placementIndicator; 

    private GameObject placedObject; 
    private ARRaycastManager raycaster; 
    private ARPlaneManager planecaster;

    private Pose placementPose; 
    private bool placementPoseIsValid = false; 

    void Start()
    {
        raycaster = FindObjectOfType<ARRaycastManager>(true); 
        planecaster = FindObjectOfType<ARPlaneManager>(true);
        raycaster.enabled = true;
        planecaster.enabled = true;
    }

    void Update()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); 
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycaster.Raycast(screenCenter, hits, TrackableType.Planes))
        {
            placementPoseIsValid = true;
            placementPose = hits[0].pose;

            var cameraForward = Camera.main.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

            // 🟢 Auto-place object when indicator is active
            if (placedObject == null) 
            {
                PlaceObject();
            }
        }
        else
        {
            placementPoseIsValid = false;
            placementIndicator.SetActive(false);
        }
    }

    void PlaceObject()
    {
        if (placementPoseIsValid)
        {
            placedObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
        }
    }
}
