using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAnimation : MonoBehaviour
{
    [System.Serializable]
    public class ExplodablePart
    {
        public Transform part;
        public Vector3 customDirection;
        public float customDistance;
    }

    public List<ExplodablePart> explodableParts = new List<ExplodablePart>();
    public float explosionSpeed = 1f;

    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private bool isExploded = false;
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private void Start()
    {
        foreach (var explodablePart in explodableParts)
        {
            if (explodablePart.part != null)
            {
                originalPositions[explodablePart.part] = explodablePart.part.localPosition;
            }
        }
    }

    // ONE button function for toggling
    public void ToggleAnimation()
    {
        // First, stop any existing animations
        StopAllAnimations();

        // Start new animation depending on the state
        if (isExploded)
        {
            activeCoroutines.Add(StartCoroutine(Assemble()));
        }
        else
        {
            activeCoroutines.Add(StartCoroutine(Explode()));
        }

        // Toggle the explosion state
        isExploded = !isExploded;
    }

    private void StopAllAnimations()
    {
        foreach (var coroutine in activeCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();
    }

    private IEnumerator Explode()
    {
        foreach (var explodablePart in explodableParts)
        {
            if (explodablePart.part != null)
            {
                Vector3 targetPosition = explodablePart.part.localPosition + explodablePart.customDirection.normalized * explodablePart.customDistance;
                activeCoroutines.Add(StartCoroutine(MovePart(explodablePart.part, targetPosition)));
            }
        }
        yield return null;
    }

    private IEnumerator Assemble()
    {
        foreach (var explodablePart in explodableParts)
        {
            if (explodablePart.part != null && originalPositions.ContainsKey(explodablePart.part))
            {
                Vector3 targetPosition = originalPositions[explodablePart.part];
                activeCoroutines.Add(StartCoroutine(MovePart(explodablePart.part, targetPosition)));
            }
        }
        yield return null;
    }

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

        part.localPosition = targetPosition;
    }
}
