using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [Serializable]
    public enum SoundType
    {
        sfx,
        music
    }

    [HideInInspector] public AudioSource source;
    public string Name;
    public AudioClip Clip;
    public SoundType Type;
    public bool Loop;

    [Range(0f, 1f)] public float Volume;
    [Range(0.1f, 3f)] public float Pitch;
}