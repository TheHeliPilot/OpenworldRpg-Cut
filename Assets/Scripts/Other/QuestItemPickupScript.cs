using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestItemPickupScript : MonoBehaviour
{
    public Quest quest;
    public int partActivation;
    public string pickupText;


    public GameObject pickupTextObject;
    
    
    [FormerlySerializedAs("_canPickup")] public bool canPickup = false;

    private void Update()
    {
        pickupTextObject.SetActive(canPickup);
    }
    
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        foreach (ActiveQuest instanceQuest in MainDialogueManager.Instance.activeQuests.Where(instanceQuest => instanceQuest.quest == quest).Where(instanceQuest => instanceQuest.pos == partActivation))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canPickup = false;
    }
}
