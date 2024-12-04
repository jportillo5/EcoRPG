using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioClip backgroundMusic; // Assign your clip here

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make sure it persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManagers
            return;
        }

        // Play the music at the start
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true; // Loop the music
            musicSource.Play();
        }
        else
        {
            Debug.LogError("AudioManager: Music Source or Background Music is missing!");
        }
    }

    public void setVolume(float volume) {
        musicSource.volume = volume;
    }
}
