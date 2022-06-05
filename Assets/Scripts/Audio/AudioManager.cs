using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager Instance;
    private Sound m_CurrentMusic;

    public float GeralVolume { get; set; } = 1;
    public float MusicVolume { get; set; } = 1;
    public float SFXVolume { get; set; } = 1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
        }

        if (!PlayerPrefs.HasKey("general_audio_value"))
        {
            PlayerPrefs.SetFloat("general_audio_value", GeralVolume);
        }
        if (!PlayerPrefs.HasKey("music_audio_value"))
        {
            PlayerPrefs.SetFloat("music_audio_value", MusicVolume);
        }
        if (!PlayerPrefs.HasKey("sfx_audio_value"))
        {
            PlayerPrefs.SetFloat("sfx_audio_value", SFXVolume);
        }

        GeralVolume = PlayerPrefs.GetFloat("general_audio_value");
        MusicVolume = PlayerPrefs.GetFloat("music_audio_value");
        SFXVolume = PlayerPrefs.GetFloat("sfx_audio_value");
    }

    public void Play(AudiosList newSound, bool isMusic = false)
    {
        var s = Array.Find(sounds, sound => sound.Name == newSound.ToString());
        if (s == null)
        {
            Debug.LogWarning("Sound \"" + newSound + "\" not found");
        }

        if (isMusic)
        {
            if (m_CurrentMusic != null)
            {
                if (m_CurrentMusic == s) return;
                else m_CurrentMusic.source.Stop();
            }
            
            m_CurrentMusic = s;
        }
        s.source.Stop();
        s.source.Play();
    }

    public void ChangeGeralVolume(float newVolume)
    {
        GeralVolume = newVolume;
        PlayerPrefs.SetFloat("general_audio_value", GeralVolume);

        foreach (Sound s in sounds)
        {
            if (s.Type == Sound.SoundType.sfx) s.source.volume = s.Volume * GeralVolume * SFXVolume;
            else s.source.volume = s.Volume * GeralVolume * MusicVolume;
        }
    }

    public void ChangeSFXVolume(float newVolume)
    {
        SFXVolume = newVolume;
        PlayerPrefs.SetFloat("sfx_audio_value", SFXVolume);

        foreach (Sound s in sounds)
        {
            if (s.Type != Sound.SoundType.sfx) continue;
            s.source.volume = s.Volume * GeralVolume * SFXVolume;
        }
    }

    public void ChangeMusicVolume(float newVolume)
    {
        MusicVolume = newVolume;
        PlayerPrefs.SetFloat("music_audio_value", MusicVolume);

        foreach (Sound s in sounds)
        {
            if (s.Type != Sound.SoundType.music) continue;
            s.source.volume = s.Volume * GeralVolume * MusicVolume;
        }
    }


    public void ChangeMusicVolumeWithLerp(float volume, float lerpTime, float startVolume= -1)
    {
        if (m_CurrentMusic == null)
            return;

        if (startVolume == -1) startVolume = m_CurrentMusic.source.volume;

        StopAllCoroutines();
        StartCoroutine(LerpMusicVolume(m_CurrentMusic, startVolume, volume, lerpTime));
    }

    IEnumerator LerpMusicVolume(Sound music, float startVolume, float volume, float lerpTime)
    {
        float oldVolume = startVolume;

        volume *= music.Volume * GeralVolume * MusicVolume;
        float lerp = 0;

        while (lerp <= lerpTime)
        {
            music.source.volume = Mathf.Lerp(oldVolume, volume, lerp / lerpTime);
            yield return null;

            lerp += Time.deltaTime;
        }
    }

    public string GetCurrentMusic()
    {
        return m_CurrentMusic.Name;
    }
}
