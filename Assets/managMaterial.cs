using UnityEngine;
using TMPro;

public class managMaterial : MonoBehaviour
{
    public TextMeshPro popupText; // Reference to the 3D TextMeshPro for the popup
    public string partName; // Name of the part to display in the popup

    private Camera arCamera; // Reference to the AR Camera

    void Start()
    {
        // Cache the AR Camera
        arCamera = Camera.main;

        // Ensure the popup text is initially hidden
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check for touch input or mouse click
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchOrClick(touch.position);
            }
        }
        else if (Input.GetMouseButtonDown(0)) // For mouse click (useful for testing in Editor)
        {
            HandleTouchOrClick(Input.mousePosition);
        }
    }

    void HandleTouchOrClick(Vector3 inputPosition)
    {
        // Perform a raycast from the input position
        Ray ray = arCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log($"Raycast hit: {hit.transform.name}");
            if (hit.transform == transform)
            {
                ShowPopup();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    void ShowPopup()
    {
        // Show the popup text with the part name
        if (popupText != null)
        {
            popupText.text = $"You touched: {partName}";
            popupText.gameObject.SetActive(true);

            // Optionally, hide the popup after a delay
            Invoke(nameof(HidePopup), 2f); // Hide after 2 seconds
        }
    }

    void HidePopup()
    {
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }
}