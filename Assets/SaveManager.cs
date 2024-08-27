using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MainManagers;
using Other;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class QuestData
{
    public GameObject fight;
    public string questName;
    public List<Conversation> conversations;
    
    public string[] choiceTexts;
    public ChoiceQuest[] choiceQuests;
    public InventoryItem[] rewards;
    public List<QuestData> neededQuests = new();

    public QuestData(Quest q)
    {
        fight = q.fight;
        questName = q.questName;
        conversations = q.conversations;
        choiceTexts = q.choiceTexts;
        choiceQuests = q.choiceQuests;
        rewards = q.rewards;
        
        foreach (Quest qNeededQuest in q.neededQuests)
        {
            neededQuests.Add(new QuestData(qNeededQuest));
        }
    }

    public Quest GetData()
    {
        Quest q = ScriptableObject.CreateInstance<Quest>();
        q.fight = fight;
        q.questName = questName;
        q.conversations = conversations;
        q.choiceTexts = choiceTexts;
        q.choiceQuests = choiceQuests;
        q.rewards = rewards;
        Quest[] quests = new Quest[neededQuests.Count];
        for (int i = 0; i < neededQuests.Count; i++)
        {
            quests[i] = neededQuests[i].GetData();
        }
        return q;
    }
}

[Serializable]
public class ItemInInventoryData
{
    public int index;
    public bool equipped;

    public ItemInInventoryData(int index, bool equipped)
    {
        this.index = index;
        this.equipped = equipped;
    }
}
[Serializable]
public class ActiveQuestData
{
    public int index;
    public int pos;

    public ActiveQuestData(int index, int pos)
    {
        this.index = index;
        this.pos = pos;
    }
}

[Serializable]
public class PartyMemberData
{
    public PartyMemberData(int speaker, int[] health, StatPair[] skills, int mainHand, int offHand, int criticalFailChance)
    {
        this.speakerId = speaker;
        this.health = new int[health.Length];
        maxHealth = new int[health.Length];
        for (int i = 0; i < health.Length; i++)
        {
            this.health[i] = health[i];
            maxHealth[i] = health[i];
        }
        baseSkills = skills;
        this.mainHand = mainHand;
        this.offHand = offHand;
        stamina = 20;
        criticalFailPercentage = criticalFailChance;
    }
    
    public int speakerId;
    public int[] health;
    public int[] maxHealth;
    public StatPair[] baseSkills;
    public int mainHand;
    public int offHand;
    public bool canAct;
    public bool isDead;
    //public List<InFightState> states = new();
    public int stamina;
    public int criticalFailPercentage;
}

[Serializable]
public class SaveData
{
    public List<int> quests;
    public List<ActiveQuestData> activeQuests;
    public List<int> finishedQuests;
    
    public List<ItemInInventoryData> inventory;
    public int[] equippedItems;
    public float[] skills;
    public List<PartyMemberData> party;
    
    public CustomDate currentDate;

    public float playerPosX;
    public float playerPosY;

    public SaveData(List<int> quests, List<ActiveQuestData> activeQuests, List<ItemInInventoryData> inventory, 
        int[] equippedItems, float[] skills, List<PartyMemberData> party, CustomDate currentDate, float playerPosX, float playerPosY, 
        List<int> finishedQuests)
    {
        this.quests = quests;
        this.activeQuests = activeQuests;
        this.inventory = inventory;
        this.equippedItems = equippedItems;
        this.skills = skills;
        this.party = party;
        this.currentDate = currentDate;
        this.playerPosX = playerPosX;
        this.playerPosY = playerPosY;
        this.finishedQuests = finishedQuests;
    }
}

public static class SaveManager
{
    //Application.persistentDataPath + "/savedata.zues"
    
    public static SaveData data;
    
    public static void Save(string filePath, SaveData objectToWrite, bool append = false)
    {
        using Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create);
        BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        binaryFormatter.Serialize(stream, objectToWrite);
        Debug.Log("Saved to " + filePath);
    }

    public static SaveData Load(string filePath)
    {
        try
        {
            using Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Debug.Log("Loading");
            return (SaveData)binaryFormatter.Deserialize(stream);
        }
        catch
        {
            Debug.Log("Starting fresh game...");
            return null;
        }
    }
    
    public static void SaveCGs(string filePath, bool[] dta, bool append = false)
    {
        using Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create);
        BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        binaryFormatter.Serialize(stream, dta);
        Debug.Log("Saved to " + filePath);
    }

    public static void LoadCGs(string filePath, int size)
    {
        try
        {
            using Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Debug.Log("Loading");
            MainMenuManagerScript.cgUnlock = (bool[])binaryFormatter.Deserialize(stream);
        }
        catch
        {
            Debug.Log("No cg savefile");
            MainMenuManagerScript.cgUnlock = new bool[size];
        }
    }
}
