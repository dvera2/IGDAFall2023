using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSfxTester : MonoBehaviour
{

    public AudioClip[] Clips;
    public AudioSource AudioSource;

    public void Start()
    {
        StartCoroutine(Play());
    }
    public IEnumerator Play()
    {
        while(true)
        {
            AudioSource.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
            yield return null;
            yield return new WaitUntil(() => !AudioSource.isPlaying);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
