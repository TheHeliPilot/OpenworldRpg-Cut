using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MainManagers;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;
using Random = UnityEngine.Random;


public class AudioCue : ScriptableObject
{
    private const string SaveFileName = "AudioCueData.json";

    [SerializeField] public AudioClip[] sounds;
    public float volume = 1;
    
    private bool _initialized = false;

    private void OnEnable()
    {
        if (!_initialized)
        {
            _initialized = true;
        }
    }

    public void OnCreate(UnityEngine.Object[] selectionObjects)
    {
        List<AudioClip> clips = new List<AudioClip>();

        if (selectionObjects.Length > 0)
        {
            clips.AddRange(selectionObjects.OfType<AudioClip>());
            sounds = clips.ToArray();
        }

        Debug.Log(sounds.Length);
    }
    public AudioClip GetSound(int i = -1)
    {
        if (sounds != null && sounds.Length != 0) return i == -1 ? sounds[Random.Range(0, sounds.Length)] : sounds[i];
        Debug.LogError("No audio clips available. Please run OnCreate method.");
        return null;

    }
}