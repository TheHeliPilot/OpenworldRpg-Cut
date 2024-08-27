using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;
using static DayNightCycleScript;
using Random = UnityEngine.Random;

public class ChurchScript : MonoBehaviour
{
    public bool shouldDing = true;
    public AudioClip ding;
    
    private int _lastHour = -1;

    private void Update()
    {
        if (_lastHour == instance.currentDate.hours) return;
        int x = _lastHour;
        _lastHour = instance.currentDate.hours;
        
        if(!(x == 23 && _lastHour == 0))
            if(x + 1 != _lastHour) return;

        x++;
        
        if(x is (> 0 and < 8) or (> 21 and <= 23))
            return;
        
        if(x > 12)
            x -= 12;

        for (int i = 0; i < (x == 0 ? 12 : x); i++)
        {
            Invoke(nameof(Ding), i * 2 + Random.Range(-0.1f, 0.1f));
        }
    }

    private void Ding()
    {
        if (!shouldDing) return;
        MainAudioManager.instance.SpawnAudio(ding, transform.position, 1, 90);
    }
}
