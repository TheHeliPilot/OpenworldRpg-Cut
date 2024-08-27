using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;

public class LockedDoorScript : MonoBehaviour
{
    public AudioCue lockedDoor;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            MainAudioManager.instance.SpawnAudio(lockedDoor.GetSound(), transform.position, lockedDoor.volume);
    }
}
