using System;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ChoiceQuest
{
    public Skills skill;
    public int minThrow = -1;
    public Quest success;
    public Quest fail;
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Dialogue/New Quest")]
public class Quest : ScriptableObject
{
    public int index;
    //if empty its not a fight!
    public GameObject fight;
    public string questName;
    public List<Conversation> conversations;
    
    public string[] choiceTexts;
    public ChoiceQuest[] choiceQuests;
    public InventoryItem[] rewards;
    public Quest[] neededQuests;
}