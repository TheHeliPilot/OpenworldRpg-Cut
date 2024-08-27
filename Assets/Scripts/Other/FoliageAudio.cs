using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FoliageAudio : MonoBehaviour
{
    public AudioCue sound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        MainAudioManager.instance.SpawnAudio(sound.GetSound(), transform.position, sound.volume);
    }
}
