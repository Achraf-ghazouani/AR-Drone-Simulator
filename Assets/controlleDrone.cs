using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class controlleDrone : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float ascendSpeed = 3f;
    public bl_Joystick leftJoystick; // Left joystick for forward/backward and rotation
    public bl_Joystick rightJoystick; // Right joystick for up/down movement
    public TextMeshProUGUI speedText; // Reference to TMP UI element for displaying speed

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        MoveDrone();
        UpdateSpeedUI();
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

    void UpdateSpeedUI()
    {
        // Calculate the magnitude of the velocity vector to get the speed
        float speed = rb.velocity.magnitude;

        // Update the TMP text with the speed value
        speedText.text = $"Speed: {speed:F2} Km/H";
    }
}