using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using Other;
using ScriptableObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class QuestSelect
{
    public Quest quest;
    public int[] questParts;
}

[Serializable]
public class MoveObject
{
    public GameObject gObject;
    public List<Vector2> pos;
    public float speed;
}

public class TriggerHandlerScript : MonoBehaviour
{
    public TriggerScript trigger;

    [Header("Quest veci")]
    public bool justSpeak;
    public QuestSelect[] quests;
    public Quest[] startQuests;
    public QuestSelect[] moveQuests;
    public GameObject CG;
    [Header("Audio")] 
    public bool audioChanges;
    public bool playMusic;
    [Header("Praca s objektami")]
    public GameObject[] showObjects;
    public GameObject[] hideObjects;
    public GameObject[] deleteObjects;
    public List<MoveObject> moveObjects;
    public GameObject[] showObjectsOnFinishMove;
    public GameObject[] hideObjectsOnFinishMove;

    [Header("Inventory veci")]
    public InventoryItem[] giveItems;
    //public InventoryItem[] takeItems;
    [Header("Party + player")]
    public PartyMemberScriptableObject[] addMembers;
    public PartyMemberScriptableObject[] removeMembers;
    public bool forceTeleport;
    public Vector2 teleportPlayer;
    [Header("Tooltip")] 
    public string popoutTooltipText;
    public Tutorial[] tutorial;
    
    private void Start()
    {
        trigger.triggeredEventHandler += OnTrigger;
    }

    private void FixedUpdate()
    {
        if (moveObjects.Count <= 0 || !_wasTriggered) return;

        List<MoveObject> mObj = moveObjects.ToList();
        foreach (MoveObject moveObject in moveObjects)
        {
            moveObject.gObject.transform.localPosition = Vector2.MoveTowards(moveObject.gObject.transform.localPosition, moveObject.pos[0], moveObject.speed);
            
            if (!(Vector2.Distance(moveObject.gObject.transform.localPosition, moveObject.pos[0]) < .1f)) continue;
            
            moveObject.pos.RemoveAt(0);
            if(moveObject.pos.Count == 0)
                mObj.Remove(moveObject);
        }

        moveObjects = mObj.ToList();
        
        if (mObj.Count != 0) return;
        foreach (GameObject showObject in showObjectsOnFinishMove)
        {
            showObject.SetActive(true);
        }
        foreach (GameObject hideObject in hideObjectsOnFinishMove)
        {
            hideObject.SetActive(false);
        }
    }

    private bool _wasTriggered;
    public void OnTrigger(object sender, EventArgs e)
    {
        _wasTriggered = true;
        if(popoutTooltipText != "")
            TooltipSystem.ShowPopoutTooltip(popoutTooltipText);

        if (audioChanges)
        {
            //audio changes here
            MainAudioManager.instance.playMusic = playMusic;
        }

        foreach (GameObject showObject in showObjects)
        {
            showObject.SetActive(true);
        }

        foreach (GameObject hideObject in hideObjects)
        {
            hideObject.SetActive(false);
        }

        foreach (GameObject deleteObject in deleteObjects)
        {
            Destroy(deleteObject);
        }
        
        foreach (Quest startQuest in startQuests)
        {
            if(MainDialogueManager.Instance.CheckFinishedQuests(startQuest.neededQuests))
                MainDialogueManager.Instance.StartQuest(startQuest);
        }

        foreach (InventoryItem inventoryItem in giveItems)
        {
            InventoryManagerScript.instance.AddItem(inventoryItem);
        }

        foreach (QuestSelect questSelect in moveQuests)
        {
            MainDialogueManager.Instance.GetActiveQuest(questSelect.quest).pos += questSelect.questParts[0];
        }

        foreach (PartyMemberScriptableObject partyMemberScriptableObject in addMembers)
        {
            InventoryManagerScript.instance.AddPartyMember(partyMemberScriptableObject);
        }

        foreach (PartyMemberScriptableObject partyMemberScriptableObject in removeMembers)
        {
            InventoryManagerScript.instance.RemovePartyMember(partyMemberScriptableObject);
        }
        
        if(tutorial.Length > 0)
            TutorialManager.instance.StartTutorial(tutorial[0]);
        
        if(teleportPlayer != new Vector2(0,0))
            InventoryManagerScript.instance.player.GetComponent<PlayerMovement>().Teleport(teleportPlayer, forceTeleport);

        if (CG != null)
        {
            Instantiate(CG, MainDialogueManager.Instance.CGObject.transform);
            CGManager.IsInCG = true;
        }
        
        if (justSpeak)
        {
            Invoke(nameof(Select), 0.1f);
            return;
        }
        
        if (quests.Length <= 0) return;

        int counter = 0;
        MainDialogueManager.Instance.isChoosing = true;
        foreach (Transform o in MainDialogueManager.Instance.choicesUIContent.transform)
        {
            Destroy(o.gameObject);
        }

        ChoiceButtonScript cbs;
        GameObject g;
        foreach (QuestSelect questSelect in quests)
        {
            g = Instantiate(MainDialogueManager.Instance.choiceObject,
                MainDialogueManager.Instance.choicesUIContent.transform);
            cbs = g.GetComponent<ChoiceButtonScript>();
            cbs.choice = counter;
            cbs.ths = this;
            cbs.isInteractChoice = true;
            ActiveQuest q = MainDialogueManager.Instance.GetActiveQuest(questSelect.quest);
            counter++;
            
            //no interaction choice for inactive quests
            if (q == null)
            {
                Destroy(g);
                continue;
            }

            if (questSelect.questParts.Length > 0)
            {
                if (!questSelect.questParts.Contains(q.pos))
                {
                    Destroy(g);
                    continue;
                }
            }

            cbs.GetComponentInChildren<TMP_Text>().text = q == null
                ? questSelect.quest.conversations[0].choiceText
                : q.quest.conversations[q.pos].choiceText;
        }
        g = Instantiate(MainDialogueManager.Instance.choiceObject,
            MainDialogueManager.Instance.choicesUIContent.transform);
        cbs = g.GetComponent<ChoiceButtonScript>();
        cbs.choice = -1;
        cbs.ths = this;
        cbs.isInteractChoice = true;
        cbs.GetComponentInChildren<TMP_Text>().text = "Back";
    }

    private void Select()
    {
        Selected(0);
    }
    
    public void Selected(int choice)
    {
        MainDialogueManager.Instance.isChoosing = false;
        if (choice == -1) return;

        //Debug.Log(choice);
        MainDialogueManager.Instance.ContinueQuest(quests[choice].quest);
    }
}