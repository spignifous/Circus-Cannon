using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(.1f, 3f)] public float pitch = 1f;
    [Range(0f, 1f)] public float pitchModifier = 0f;
    public bool pitchSum;

    public bool loop;
    public bool mute;

    [HideInInspector] public AudioSource source;

    [HideInInspector] public float pitchSumStartTime;
    [HideInInspector] public float pitchSumTemp;
}
