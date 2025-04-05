using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using UnityEngine.UI; // Import UI namespace for Button

public class controlleDrone : MonoBehaviour
{
    public float moveSpeed = 5f; // Default move speed
    public float rotationSpeed = 2f;
    public float ascendSpeed = 3f;
    public bl_Joystick leftJoystick; // Left joystick for forward/backward and rotation
    public bl_Joystick rightJoystick; // Right joystick for up/down movement
    public TextMeshProUGUI speedText; // Reference to TMP UI element for displaying speed
    public TMP_InputField maxSpeedInputField; // Input field for max speed
    public Button startButton; // Reference to the Start button
    public Button closeButton; // Reference to the Close button

    private Rigidbody rb;
    private bool isControllable = false; // Tracks if the drone can be controlled

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set input field to accept only numeric input
        maxSpeedInputField.contentType = TMP_InputField.ContentType.DecimalNumber;

        // Assign button click listeners
        startButton.onClick.AddListener(OnStartButtonPressed);
        closeButton.onClick.AddListener(OnCloseButtonPressed);

        // Assign input field listener
        maxSpeedInputField.onEndEdit.AddListener(OnMaxSpeedEntered);
    }

    void FixedUpdate()
    {
        if (isControllable)
        {
            MoveDrone();
            UpdateSpeedUI();
        }
        else
        {
            // Stop the drone's movement when not controllable
            rb.velocity = Vector3.zero;
        }
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

    // Method to handle the Start button press
    public void OnStartButtonPressed()
    {
        isControllable = true; // Enable drone control
    }

    // Method to handle the Close button press
    public void OnCloseButtonPressed()
    {
        isControllable = false; // Disable drone control
    }

    // Method to handle max speed input
    public void OnMaxSpeedEntered(string input)
    {
        if (float.TryParse(input, out float newMaxSpeed))
        {
            // Clamp the speed to a maximum of 50
            moveSpeed = Mathf.Clamp(newMaxSpeed, 0, 50);
        }
        else
        {
            Debug.LogWarning("Invalid speed entered. Please enter a valid number.");
        }

        // Update the input field to reflect the clamped value
        maxSpeedInputField.text = moveSpeed.ToString();
    }
}