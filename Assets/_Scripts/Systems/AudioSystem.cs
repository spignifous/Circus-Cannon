using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : StaticInstance<AudioSystem>
{
    [Space(3)]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _volumeMaster;
    [SerializeField] private AudioMixerGroup _volumeMusic;
    [SerializeField] private AudioMixerGroup _volumeSFX;

    [Space(3)]
    public Sound[] music;
    [Space(3)]
    public Sound[] sounds;

    private Sound[] soundAndMusic;

    public string MusicCurrent { get; private set; }

    private List<Sound> _pitchSum = new List<Sound>();

    #region UNITY FUNCTIONS

    protected override void Awake()
    {
        base.Awake();

        // Concat
        soundAndMusic = music.Concat(sounds).ToArray();

        foreach (Sound snd in soundAndMusic)
        {
            snd.source = gameObject.AddComponent<AudioSource>();
            snd.source.clip = snd.clip;

            snd.source.volume = snd.volume;
            snd.source.pitch = snd.pitch;

            snd.pitchSumTemp = snd.pitch;

            snd.source.loop = snd.loop;
            snd.source.mute = snd.mute;

            if (snd.pitchSum)
            {
                _pitchSum.Add(snd);
            }

            if (music.Contains(snd))
            {
                if (_volumeMusic != null) 
                    snd.source.outputAudioMixerGroup = _volumeMusic;
            }
            else
            {
                if (_volumeSFX != null)
                    snd.source.outputAudioMixerGroup = _volumeSFX;
            }
        }

        // Audio Play
        Play("Music");
    }

    private void Update()
    {
        PitchReset();
    }

    #endregion

    #region BASIC FUNCTIONS

    public Sound Play(Sound snd, Vector3 position = default)
    {
        if (snd != null)
        {
            if (snd.source.enabled)
            {
                if (position != default)
                    snd.source.transform.position = position;

                PitchModifier(snd, Time.time);

                snd.source.Play();

                if (music.Contains(snd))
                {
                    MusicCurrent = snd.name;
                }
            }
        }

        return snd;
    }

    private void PitchModifier(Sound snd, float startTime)
    {
        if (snd.pitchSum)
        {
            snd.pitchSumStartTime = startTime;

            if (snd.source.pitch < 1.8f)
            {
                snd.source.pitch += snd.pitchModifier;
            }
        }
        else
        {
            float modifier = UnityEngine.Random.Range(-snd.pitchModifier, snd.pitchModifier);
            snd.source.pitch = snd.pitch + modifier;
        }
    }

    private void PitchReset()
    {
        foreach (Sound snd in _pitchSum)
        {
            if (Time.time > snd.pitchSumStartTime + .5f)
            {
                snd.source.pitch = snd.pitch;
            }
        }
    }

    private Sound SoundFind(string name)
    {
        Sound snd = Array.Find(soundAndMusic, sound => sound.name == name);
        if (snd == null)
        {
            Debug.LogWarning("Sound '" + name + "' not found!");
        }

        return snd;
    }

    public Sound Play(string name,  Vector3 position = default)
    {
        Sound snd = SoundFind(name);

        return Play(snd, position);
    }

    #endregion
}
