using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    public GameObject objectToPlace; // Model of the Object we want to place, in this case the turbine
    public GameObject placementIndicator; // A Indicator where we place the Object, in this case the "Placement" object we created in the Hierarchy
    private GameObject placedObject; // The Object we created at the Indicator position
    private ARRaycastManager raycaster; // AR Components on XR Origin
    private ARPlaneManager planecaster;

    private Pose placementPose; // Rotation and Position of the Indicator
    private bool placementPoseIsValid = false; // Can we place the Object on this Place (is a Plane detected etc.)

    void Start()
    {
        raycaster = FindObjectOfType<ARRaycastManager>(true); // Populating AR Components automatically
        planecaster = FindObjectOfType<ARPlaneManager>(true);
        raycaster.enabled = true; // Enable the Components (safety meassure)
        planecaster.enabled = true;
    }

    void Update()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)); //3D Point in the Middle of the Screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>(); // List of points we hit with the Raycast

        raycaster.Raycast(screenCenter, hits, TrackableType.Planes); // The bool ARRaycastManager.Raycast(args) sends a ray
                                                                     // from the AR Camera through a point (in this case the middle of the screen).
                                                                     // When this Ray hits one or more AR Features (here we aim for points on planes)
                                                                     // it returns true and safes each of the hitpoints in a "ARRaycastHit" object.
                                                                     // A ARRaycastHit contains valuable information like name of the plane or position
                                                                     // and rotation of the feature (here of the plane point). 

        placementPoseIsValid = hits.Count > 0; // We say the placement pose is valid when we hit at least one plane.
                                               // placementPoseIsValid =  raycaster.Raycast(screenCenter, hits, TrackableType.Planes); would access the
                                               // bool directly and should also be possible in most cases.
                                               // Again, we check the count to be sure

        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose; // access the pose of the first hit Plane

            var cameraForward = Camera.current.transform.forward; // The direction Vector the AR Camera is looking in
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized; // Normalize the Direction Vector and flatening it on the x-z plane
            placementPose.rotation = Quaternion.LookRotation(cameraBearing); // Rotate the placement pose so it looks in the same direction as the camera
                                                                             // With this the placed Turbine always oriented in direction of the Camera

            placementIndicator.SetActive(true); // Make the Indicator visible
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation); //Overlap the pose of the Indicator
                                                                                                                 //with the placement pose
        }
        else
        {
            placementIndicator.SetActive(false); // deactivate the Indicator when we dont hit a plane
        }
    }

    public void PlaceObject() // Function to call when button is pressed
    {
        if (placementPoseIsValid) // check if we hit a plane
        {
            if (placedObject != null) // destroy old turbine if there already exists one
                Destroy(placedObject);

            placedObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation); // Clone a turbine at the
                                                                                                       // position of the placement
                                                                                                       // pose with the right rotation
        }
    }
}
