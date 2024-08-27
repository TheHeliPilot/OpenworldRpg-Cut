using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoEndTriggerHandler : MonoBehaviour
{
    public TriggerScript trigger;
    
    private void Start()
    {
        trigger.triggeredEventHandler += OnTrigger;
    }
    
    public void OnTrigger(object sender, EventArgs e)
    {
        SceneManager.LoadScene(0);
    }
}
