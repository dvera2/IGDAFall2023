using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Bank", menuName = "Audio Bank")]
public class AudioBank : ScriptableObject
{
    public AudioClip[] AudioClips;

    public void Play(AudioSource src)
    {
        if (AudioClips.Length == 0)
            return;

        if (src == null)
            return;

        src.PlayOneShot(AudioClips[Random.Range(0,AudioClips.Length)]);
    }
}
