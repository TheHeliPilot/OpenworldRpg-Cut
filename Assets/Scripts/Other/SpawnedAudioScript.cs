using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpawnedAudioScript : MonoBehaviour
{
    public void Setup(AudioClip clip, bool loop = false, float volumeMult = 1f)
    {

        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = loop;
        GetComponent<AudioSource>().volume *= volumeMult;

        if (loop)
            return;

        float length = clip.length + 1;
        Destroy(gameObject, length);
    }
}