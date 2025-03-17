using UnityEngine;

public class controlleDrone : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float ascendSpeed = 3f;
    public bl_Joystick leftJoystick; // Left joystick for forward/backward and rotation
    public bl_Joystick rightJoystick; // Right joystick for up/down movement
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveDrone();
    }

    void MoveDrone()
    {
        // Get joystick inputs
        float moveZ = leftJoystick.Vertical;  // Forward/Backward movement
        float rotation = leftJoystick.Horizontal * rotationSpeed; // Rotate Left/Right
        float moveY = rightJoystick.Vertical * ascendSpeed; // Ascend/Descend

        // Calculate movement
        Vector3 moveDirection = transform.forward * moveZ + Vector3.up * moveY;
        rb.velocity = moveDirection * moveSpeed;

        // Rotate the drone based on left joystick horizontal input
        transform.Rotate(Vector3.up * rotation);
    }
}
