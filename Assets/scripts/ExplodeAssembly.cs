using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAssemblyWithManualParameters : MonoBehaviour
{
    [System.Serializable]
    public class ExplodablePart
    {
        public Transform part; // Das Bauteil
        public Vector3 customDirection; // Die benutzerdefinierte Richtung
        public float customDistance; // Der Abstand in der Explosionsdarstellung
    }

    // Liste aller explodierbaren Teile mit ihren Parametern
    public List<ExplodablePart> explodableParts = new List<ExplodablePart>();

    // Geschwindigkeit der Explosion
    public float explosionSpeed = 1f;

    // Originalpositionen der Bauteile
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();

    // Flag für den Explosionszustand
    private bool isExploded = false;

    private void Start()
    {
        // Originalpositionen aller Teile speichern
        foreach (var explodablePart in explodableParts)
        {
            if (explodablePart.part != null)
            {
                originalPositions[explodablePart.part] = explodablePart.part.localPosition;
            }
        }
    }

    // Funktion zum Umschalten zwischen Explosionszustand und Zusammenbau
    public void ToggleExplosion()
    {
        if (isExploded)
        {
            StartCoroutine(Assemble());
        }
        else
        {
            StartCoroutine(Explode());
        }

        isExploded = !isExploded; // Zustand umschalten
    }

    // Coroutine für die Explosion
    private IEnumerator Explode()
    {
        foreach (var explodablePart in explodableParts)
        {
            if (explodablePart.part != null)
            {
                // Zielposition berechnen: Teil bewegt sich in die benutzerdefinierte Richtung
                Vector3 targetPosition = explodablePart.part.localPosition + explodablePart.customDirection.normalized * explodablePart.customDistance;

                // Animierte Bewegung
                StartCoroutine(MovePart(explodablePart.part, targetPosition));
            }
        }

        yield return null;
    }

    // Coroutine für den Zusammenbau
    private IEnumerator Assemble()
    {
        foreach (var explodablePart in explodableParts)
        {
            if (explodablePart.part != null)
            {
                // Zurück zur Originalposition
                Vector3 targetPosition = originalPositions[explodablePart.part];

                // Animierte Bewegung
                StartCoroutine(MovePart(explodablePart.part, targetPosition));
            }
        }

        yield return null;
    }

    // Bewegung eines Teils zu einer Zielposition
    private IEnumerator MovePart(Transform part, Vector3 targetPosition)
    {
        Vector3 startPosition = part.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / explosionSpeed)
        {
            elapsedTime += Time.deltaTime * explosionSpeed;
            part.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }

        part.localPosition = targetPosition; // Endposition sicherstellen
    }
}
