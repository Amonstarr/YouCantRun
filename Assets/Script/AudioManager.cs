using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip buttonClickSFX;
    
    [Header("Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 0.7f;
    [SerializeField] private bool playMusicOnStart = true;
    
    private static AudioManager instance;
    
    private void Awake()
    {
        // Singleton pattern - jaga agar tidak hilang saat pindah scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Setup audio sources
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.volume = musicVolume;
        }
        
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }
    }
    
    private void Start()
    {
        if (playMusicOnStart && mainMenuMusic != null)
        {
            PlayMusic(mainMenuMusic);
        }
    }
    
    // Play music
    public static void PlayMusic(AudioClip clip)
    {
        if (instance != null && instance.musicSource != null && clip != null)
        {
            if (instance.musicSource.clip == clip && instance.musicSource.isPlaying)
                return;
            
            instance.musicSource.clip = clip;
            instance.musicSource.Play();
        }
    }
    
    // Stop music
    public static void StopMusic()
    {
        if (instance != null && instance.musicSource != null)
        {
            instance.musicSource.Stop();
        }
    }
    
    // Pause music
    public static void PauseMusic()
    {
        if (instance != null && instance.musicSource != null)
        {
            instance.musicSource.Pause();
        }
    }
    
    // Resume music
    public static void ResumeMusic()
    {
        if (instance != null && instance.musicSource != null)
        {
            instance.musicSource.UnPause();
        }
    }
    
    // Play sound effect
    public static void PlaySFX(AudioClip clip)
    {
        if (instance != null && instance.sfxSource != null && clip != null)
        {
            instance.sfxSource.PlayOneShot(clip);
        }
    }
    
    // Play button click sound
    public static void PlayButtonClick()
    {
        if (instance != null && instance.buttonClickSFX != null)
        {
            PlaySFX(instance.buttonClickSFX);
        }
    }
    
    // Set music volume
    public static void SetMusicVolume(float volume)
    {
        if (instance != null && instance.musicSource != null)
        {
            instance.musicVolume = Mathf.Clamp01(volume);
            instance.musicSource.volume = instance.musicVolume;
        }
    }
    
    // Set SFX volume
    public static void SetSFXVolume(float volume)
    {
        if (instance != null && instance.sfxSource != null)
        {
            instance.sfxVolume = Mathf.Clamp01(volume);
            instance.sfxSource.volume = instance.sfxVolume;
        }
    }
    
    // Get music volume
    public static float GetMusicVolume()
    {
        return instance != null ? instance.musicVolume : 0f;
    }
    
    // Get SFX volume
    public static float GetSFXVolume()
    {
        return instance != null ? instance.sfxVolume : 0f;
    }
}
