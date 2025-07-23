using System;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{
    [Range(0f, 1f)]
    public float BGMVolume = 1f;
    [Range(0f, 1f)]
    public float SFXVolume = 1f;

    public Sound[] BGMSoundList;
    public Sound[] SFXSoundList;

    protected override void Awake()
    {
        base.Awake();
        foreach (Sound bgmSound in BGMSoundList)
        {
            bgmSound.audioSource = gameObject.AddComponent<AudioSource>();
            bgmSound.audioSource.clip = bgmSound.audioClip;
            bgmSound.audioSource.loop = true;
        }
        foreach (Sound sfxSound in SFXSoundList)
        {
            sfxSound.audioSource = gameObject.AddComponent<AudioSource>();
            sfxSound.audioSource.clip = sfxSound.audioClip;
            sfxSound.audioSource.loop = false;
        }

        PlayBGM(AudioType.Theme);
    }

    public void PlayBGM(AudioType type)
    {
        Sound bgm = Array.Find(BGMSoundList, s => s.audioType == type);
        if (bgm != null && !bgm.audioSource.isPlaying) bgm.audioSource.Play();

        foreach (Sound bgmSound in BGMSoundList)
        {
            if (bgmSound.audioSource.isPlaying && bgmSound.audioType != type)
                bgmSound.audioSource.Stop();
        }
    }
    public void PlaySFX(AudioType type, bool loop = false)
    {
        Sound sfx = Array.Find(SFXSoundList, s => s.audioType == type);
        if (sfx != null)
        {
            if (!loop) sfx.audioSource.PlayOneShot(sfx.audioClip);
            else sfx.audioSource.Play();
        }
    }
    public void ToggleBGMState(bool state)
    {
        foreach (Sound bgmSound in BGMSoundList)
        {
            bgmSound.mute = !state;
            bgmSound.audioSource.mute = bgmSound.mute;
        }
    }
    public void ToggleSFXState(bool state)
    {
        foreach (Sound sfxSound in SFXSoundList)
        {
            sfxSound.mute = !state;
            sfxSound.audioSource.mute = sfxSound.mute;
        }
    }

    private void OnEnable()
    {
        EventManager.OnTileClick += () => PlaySFX(AudioType.TileClick);
    }
    private void OnDisable()
    {
        EventManager.OnTileClick -= () => PlaySFX(AudioType.TileClick);
    }
}

[System.Serializable]
public class Sound
{
    public AudioType audioType;
    public AudioClip audioClip;
    public AudioSource audioSource { get; set; }
    public bool loop { get; set; }
    public bool mute { get; set; }
}
