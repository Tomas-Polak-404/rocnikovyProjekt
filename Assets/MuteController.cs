using UnityEngine;

public class MuteController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found!");
        }

        // P�i startu nastav�me mute podle hodnoty statick� prom�nn� z t��dy ButtonsFces
        audioSource.mute = ButtonsFces.muteMusic;
    }

    // Metoda pro nastaven� ztlumen�/zapnut� zvuku
    public void SetMute(bool mute)
    {
        if (audioSource != null)
        {
            audioSource.mute = mute;
            ButtonsFces.muteMusic = mute; // Aktualizujeme hodnotu statick� prom�nn� v t��d� ButtonsFces
        }
        else
        {
            Debug.LogError("AudioSource component is not assigned!");
        }
    }
}
