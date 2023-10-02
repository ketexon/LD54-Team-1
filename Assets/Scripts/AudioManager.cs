using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private List<AudioClip> audioClips;

    private float masterVolume=1f;
    private float musicVolume=1f;
    private float sfxVolume=1f;

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

    public void PlayButtonSFX()
    {
        PlaySFX(4);
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

    public void ChangeMasterVolume(Slider s)
    {
        masterVolume = s.value;
        UpdateVolumes();
    }

    public void ChangeMusicVolume(Slider s)
    {
        musicVolume = s.value;
        UpdateVolumes();
    }

    public void ChangeSFXVolume(Slider s)
    {
        sfxVolume = s.value;
        UpdateVolumes();
    }
}
