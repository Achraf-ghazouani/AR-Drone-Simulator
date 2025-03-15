using UnityEngine;

public class RotateEngine : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public Vector3 rotationAxis = Vector3.up;
    private int rotationDirection = 0;
    private AudioSource audioSource;
    public AudioClip rotationSound;
    [SerializeField] private ParticleSystem windParticles; // Windwirbelung Partikelsystem
    [SerializeField] private float maxEmissionRate = 100f;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rotationSound;
        audioSource.loop = true;

        if (windParticles != null)
        {
            windParticles.Stop(); // Startet deaktiviert
        }
    }

    private void Update()
    {
        if (rotationDirection != 0)
        {
            RotateContinuously(rotationDirection);

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            UpdateWindEffect(true);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            UpdateWindEffect(false);
        }
    }

    public void ToggleRotationLeft()
    {
        if (rotationDirection == -1)
        {
            rotationDirection = 0;
        }
        else
        {
            rotationDirection = -1;
        }
    }

    public void ToggleRotationRight()
    {
        if (rotationDirection == 1)
        {
            rotationDirection = 0;
        }
        else
        {
            rotationDirection = 1;
        }
    }

    private void RotateContinuously(int direction)
    {
        float angle = direction * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationAxis, angle, Space.Self);
    }

    private void UpdateWindEffect(bool isActive)
    {
        if (windParticles != null)
        {
            var emission = windParticles.emission;
            emission.rateOverTime = isActive ? maxEmissionRate : 0f;

            if (isActive && !windParticles.isPlaying)
            {
                windParticles.Play();
            }
            else if (!isActive && windParticles.isPlaying)
            {
                windParticles.Stop();
            }
        }
    }
}
