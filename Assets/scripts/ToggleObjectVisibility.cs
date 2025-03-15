
using UnityEngine;
using UnityEngine.UI;

public class ToggleObjectVisibility : MonoBehaviour
{
    // Das 3D-Objekt, das ein- oder ausgeblendet werden soll
    public GameObject targetObject;

    // Die Methode, die aufgerufen wird, wenn der Toggle-Status geändert wird
    public void OnToggleChanged(Toggle toggle)
    {
        if (targetObject != null)
        {
            // Setze die Sichtbarkeit des Objekts entsprechend dem Toggle-Status
            targetObject.SetActive(toggle.isOn);
        }
        else
        {
            Debug.LogWarning("Kein Zielobjekt zugewiesen!");
        }
    }
}
