using UnityEngine;

public class PlaySoundButton : MonoBehaviour
{
    // Die AudioSource-Komponente
    private AudioSource audioSource;

    // Die MP3- oder WAV-Datei, die abgespielt werden soll
    public AudioClip soundClip;

    private void Start()
    {
        // AudioSource-Komponente hinzufügen und konfigurieren
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;
        audioSource.playOnAwake = false; // Sound soll nicht automatisch starten
        audioSource.loop = false; // Standardmäßig nicht in einer Schleife abspielen
    }

    // Diese Methode wird vom Button aufgerufen, um den Sound zu starten oder zu stoppen
    public void ToggleSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Sound stoppen
        }
        else
        {
            audioSource.Play(); // Sound starten
        }
    }
}
