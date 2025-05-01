using UnityEngine;

public class InformationPanel : MonoBehaviour
{
    public GameObject[] images; // Array to hold the images
    public GameObject panel; // Reference to the panel
    private int currentIndex = 0; // Tracks the current image index

    void Start()
    {
        // Ensure only the first image is active at the start
        UpdateImageVisibility();
    }

    public void NextImage()
    {
        if (currentIndex < images.Length - 1)
        {
            currentIndex++;
            UpdateImageVisibility();
        }
        else
        {
            ClosePanel(); // Close the panel if it's the last image
        }
    }

    public void PreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateImageVisibility();
        }
    }

    private void UpdateImageVisibility()
    {
        // Loop through all images and activate only the current one
        for (int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(i == currentIndex);
        }
    }

    public void ClosePanel()
    {
        panel.SetActive(false); // Deactivate the panel
    }

    public void OpenPanel()
    {
        // Reset to the first image when the panel is opened
        currentIndex = 0;
        UpdateImageVisibility();
        panel.SetActive(true); // Activate the panel
    }
}