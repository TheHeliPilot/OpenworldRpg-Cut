using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public bool locked;
    public GameObject strecha;
    public GameObject inside;
    public GameObject lockObject;
    public AudioCue doorCue;

    private void Update()
    {
        if(lockObject.activeSelf != locked)
            lockObject.SetActive(locked);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            strecha.SetActive(false);
            inside.SetActive(true);
            other.gameObject.GetComponent<PlayerMovement>().isInBuilding = true;
            MainAudioManager.instance.SpawnAudio(doorCue.GetSound(), transform.position, doorCue.volume);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            strecha.SetActive(true);
            inside.SetActive(false);
            other.gameObject.GetComponent<PlayerMovement>().isInBuilding = false;
            MainAudioManager.instance.SpawnAudio(doorCue.GetSound(), transform.position, doorCue.volume);
        }
    }
}
