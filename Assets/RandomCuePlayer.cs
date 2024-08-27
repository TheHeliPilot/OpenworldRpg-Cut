using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;

public class RandomCuePlayer : MonoBehaviour
{
    public AudioCue cue;

    public void Play()
    {
        MainAudioManager.instance.SpawnAudio(cue.GetSound(), Camera.main.transform.position, cue.volume, 150, false, false);
    }
}
