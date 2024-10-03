using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [Space(3)]
    public Sound[] music;
    [Space(3)]
    public Sound[] sounds;

    private Sound[] soundAndMusic;

    public string MusicCurrent { get; private set; }

    #region BASIC METHODS

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
            float modifier = Random.Range(-snd.pitchModifier, snd.pitchModifier);
            snd.source.pitch = snd.pitch + modifier;
        }
    }

    #endregion
}
