using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private List<AudioClip> audioClips;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        musicSource.PlayScheduled(AudioSettings.dspTime + .1f);
        loopSource.PlayScheduled(AudioSettings.dspTime + .1f + 1.69f);
    }

    public void PlaySFX(int which, float volume=1f)
    {
        sfxSource.PlayOneShot(audioClips[which], volume);
    }

    void UpdateVolumes()
    {
        musicSource.volume = musicVolume * masterVolume;
        loopSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    void ChangeMasterVolume(float volume)
    {
        masterVolume = volume;
        UpdateVolumes();
    }

    void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolumes();
    }

    void ChangeSFXVolume(float volume)
    {
        sfxVolume = volume;
        UpdateVolumes();
    }
}
