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

        // Pøi startu nastavíme mute podle hodnoty statické promìnné z tøídy ButtonsFces
        audioSource.mute = ButtonsFces.muteMusic;
    }

    // Metoda pro nastavení ztlumení/zapnutí zvuku
    public void SetMute(bool mute)
    {
        if (audioSource != null)
        {
            audioSource.mute = mute;
            ButtonsFces.muteMusic = mute; // Aktualizujeme hodnotu statické promìnné v tøídì ButtonsFces
        }
        else
        {
            Debug.LogError("AudioSource component is not assigned!");
        }
    }
}
