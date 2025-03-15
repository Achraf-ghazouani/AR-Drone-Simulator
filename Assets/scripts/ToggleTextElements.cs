using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextElements : MonoBehaviour
{
    [SerializeField] private List<GameObject> textElements; // Liste der zu verwaltenden Textbausteine
    [SerializeField] private Button toggleButton; // Referenz auf den Button
    private bool isVisible = false;

    void Start()
    {
        foreach (GameObject textElement in textElements)
        {
            if (textElement != null)
            {
                textElement.SetActive(false);
            }
        }

        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleTextVisibility);
        }
    }

    private void ToggleTextVisibility()
    {
        isVisible = !isVisible;
        foreach (GameObject textElement in textElements)
        {
            if (textElement != null)
            {
                textElement.SetActive(isVisible);
            }
        }
    }
}
