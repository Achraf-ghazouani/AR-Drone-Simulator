using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float tiltAngle = 20f;

    [Header("Components")]
    [SerializeField] private RotationTurbinDrone turbineController;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private void Update()
    {
        // Get input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        float upDownInput = 0;
        
        if (Input.GetKey(KeyCode.Space))
            upDownInput = 1;
        if (Input.GetKey(KeyCode.LeftShift))
            upDownInput = -1;

        // Calculate movement
        Vector3 movement = transform.forward * verticalInput * movementSpeed;
        Vector3 upDown = Vector3.up * upDownInput * verticalSpeed;
        float rotation = horizontalInput * rotationSpeed;

        // Apply movement
        rb.velocity = movement + upDown;
        transform.Rotate(Vector3.up * rotation * Time.deltaTime);

        // Tilt the drone based on forward/backward movement
        float tilt = -verticalInput * tiltAngle;
        transform.rotation = Quaternion.Euler(tilt, transform.rotation.eulerAngles.y, 0);
    }

    public void StartDrone()
    {
        if (turbineController != null)
        {
            turbineController.PlayTurbineAnimation();
        }
    }

    public void StopDrone()
    {
        if (turbineController != null)
        {
            turbineController.StopTurbineAnimation();
            rb.velocity = Vector3.zero;
        }
    }
}
