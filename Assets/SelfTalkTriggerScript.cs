using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelfTalkTriggerScript : MonoBehaviour
{
    public bool reverseTrigger;
    public Conversation text;
    public Vector2 force;
    public bool triggerOnce;
    public bool randomize;
    public TriggerScript trigger;

    private bool wasTriggered;

    private void Trigger()
    {
        MainDialogueManager.Instance.TalkToSelf(randomize ? new[] { text.GetLines()[Random.Range(0, text.GetLines().Length)] } : text.GetLines(), force);
        if(trigger != null)
            trigger.Triggered(this , EventArgs.Empty);
        
        if (triggerOnce)
            gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || reverseTrigger) return;

       Trigger();
    } 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") || !reverseTrigger) return;

        Trigger();
    }
}
