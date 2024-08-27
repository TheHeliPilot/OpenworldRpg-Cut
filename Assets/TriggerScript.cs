using System;
using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public float delay;
    public bool oneUse;
    public bool collisionTrigger;
    public bool reverseTrigger;
    public bool boundToQuestSuccess;
    public bool boundToQuestFail;
    public bool boundToQuestEvent;
    [Tooltip("can be tag for collider OR quest name for quest binds")]
    public string triggerTag;

    public KeyCode key;

    public EventHandler triggeredEventHandler;

    private void Start()
    {
        if (boundToQuestSuccess)
            MainDialogueManager.Instance.questSuccessEventHandler += Triggered;
        if (boundToQuestFail)
            MainDialogueManager.Instance.questFailEventHandler += Triggered;
        if (boundToQuestEvent)
            MainDialogueManager.Instance.questDialogueEventHandler += Triggered;
    }

    private void Update()
    {
        if (key == KeyCode.None) return;
        if(Input.GetKeyDown(key))
            Triggered(this, EventArgs.Empty);
    }

    public void Triggered(object sender, EventArgs e)
    {
        Invoke(nameof(OnTriggered), delay);
    }

    public void OnTriggered()
    {
        if (boundToQuestSuccess)
            if(MainDialogueManager.Instance.lastQuestSuccess.questName != triggerTag)
                return;
        
        if (boundToQuestFail)
            if(MainDialogueManager.Instance.lastQuestFail.questName != triggerTag)
                return;
        
        if (boundToQuestEvent)
            if(MainDialogueManager.Instance.lastQuestEventName != triggerTag)
                return;
        
        triggeredEventHandler?.Invoke(this, EventArgs.Empty);
        if(oneUse)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collisionTrigger || reverseTrigger) return;

        if (other.gameObject.CompareTag(triggerTag) && !other.isTrigger)
            Triggered(this, EventArgs.Empty);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!collisionTrigger || !reverseTrigger) return;

        if (other.gameObject.CompareTag(triggerTag) && !other.isTrigger)
            Triggered(this, EventArgs.Empty);
    }
}
