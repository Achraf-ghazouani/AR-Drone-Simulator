using UnityEngine;

public class RotateEngine_v01: MonoBehaviour
{
    // Geschwindigkeit der Rotation
    public float rotationSpeed = 100f;

    // Die Achse, um die sich das Objekt drehen soll (z.B. Vector3.up f�r die Y-Achse)
    public Vector3 rotationAxis = Vector3.up;

    // Aktuelle Rotationsrichtung: -1 (links), 1 (rechts), 0 (keine Rotation)
    private int rotationDirection = 0;

    // AudioSource-Komponente zum Abspielen des Sounds
    private AudioSource audioSource;

    // AudioClip (z. B. eine MP3-Datei), der abgespielt werden soll
    public AudioClip rotationSound;

    private void Start()
    {
        // AudioSource-Komponente initialisieren
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rotationSound;
        audioSource.loop = true; // Der Sound wird in einer Schleife abgespielt
    }

    // Update-Methode f�r kontinuierliche Rotation
    private void Update()
    {
        if (rotationDirection != 0)
        {
            RotateContinuously(rotationDirection);

            // Sound starten, wenn nicht bereits abgespielt
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Sound stoppen, wenn die Rotation stoppt
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    // Methode, um die Rotation nach links zu starten und die Rotation nach rechts zu stoppen
    public void ToggleRotationLeft()
    {
        if (rotationDirection == -1)
        {
            rotationDirection = 0; // Stoppt die Rotation
        }
        else
        {
            rotationDirection = -1; // Setzt die Rotation nach links
        }
    }

    // Methode, um die Rotation nach rechts zu starten und die Rotation nach links zu stoppen
    public void ToggleRotationRight()
    {
        if (rotationDirection == 1)
        {
            rotationDirection = 0; // Stoppt die Rotation
        }
        else
        {
            rotationDirection = 1; // Setzt die Rotation nach rechts
        }
    }

    // Logik f�r kontinuierliches Rotieren
    private void RotateContinuously(int direction)
    {
        float angle = direction * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, angle, Space.Self);
    }
}
