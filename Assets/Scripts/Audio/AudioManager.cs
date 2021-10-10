using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    public static AudioManager Instance;
    private Sound m_CurrentMusic;

    private float m_GeralVolume = 1;
    private float m_MusicVolume = 1;
    private float m_SFXVolume = 1;

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
            if (m_CurrentMusic != null) m_CurrentMusic.source.Stop();
            m_CurrentMusic = s;
        }
        s.source.Stop();
        s.source.Play();
    }

    public void ChangeGeralVolume(float newVolume)
    {
        m_GeralVolume = newVolume;
        foreach (Sound s in sounds)
        {
            if (s.Type == Sound.SoundType.sfx) s.source.volume = s.Volume * m_GeralVolume * m_SFXVolume;
            else s.source.volume = s.Volume * m_GeralVolume * m_MusicVolume;
        }
    }

    public void ChangeSFXVolume(float newVolume)
    {
        m_SFXVolume = newVolume;
        foreach (Sound s in sounds)
        {
            if (s.Type != Sound.SoundType.sfx) continue;
            s.source.volume = s.Volume * m_GeralVolume * m_SFXVolume;
        }
    }

    public void ChangeMusicVolume(float newVolume)
    {
        m_MusicVolume = newVolume;
        foreach (Sound s in sounds)
        {
            if (s.Type != Sound.SoundType.music) continue;
            s.source.volume = s.Volume * m_GeralVolume * m_MusicVolume;
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

        volume *= music.Volume * m_GeralVolume * m_MusicVolume;
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
