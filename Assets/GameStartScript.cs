using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameStartScript : MonoBehaviour
{
    public GameObject[] essentials;
    public bool skip;
    public AudioClip boomClip;
    [HideInInspector]
    public bool boom;
    public GameObject black;

    private void Start()
    {
        foreach (GameObject essential in essentials)
        {
            essential.SetActive(false);
        }
    }

    private bool a;
    private void Update()
    {
        if (boom && !a)
        {
            MainAudioManager.instance.SpawnAudio(boomClip, Vector3.zero, .1f, 1, false, false);
            a = true;
        }

        if (!skip) return;
        black.GetComponent<Image>().color = Color.black;
        foreach (GameObject essential in essentials)
        {
            essential.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
