using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using Other;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public bool freshGame;
    
    private static SaveData _data;
    
    private void Start()
    {
        Invoke(nameof(LateStart), .1f);
    }

    private void LateStart()
    {
        _data = SaveManager.Load(Application.persistentDataPath + "/savedata.zues");
        if (_data == null || freshGame)
        {
            Debug.LogWarning("Fresh game");

            _data = new SaveData(MainDialogueManager.Instance.quests.Select(i => i.index).ToList(), GetActiveQuests(),
                GetInventory(), GetEquippedItems(), InventoryManagerScript.instance.skills, 
                GetPartyMembers(), DayNightCycleScript.instance.currentDate, 
                3625.25f,2398.40991f, MainDialogueManager.Instance.finishedQuests.Select(i => i.index).ToList());
            
            SaveManager.Save(Application.persistentDataPath + "/savedata.zues", _data);
        }
        else
        {
            MainDialogueManager.Instance.quests = LoadQuests(_data.quests);
            MainDialogueManager.Instance.activeQuests = LoadActiveQuests(_data.activeQuests);
            MainDialogueManager.Instance.finishedQuests = LoadQuests(_data.finishedQuests);

            InventoryManagerScript.instance.inventory = LoadInventory(_data.inventory);
            InventoryManagerScript.instance.equippedItems = LoadEquipped(_data.equippedItems);
            InventoryManagerScript.instance.skills = _data.skills;
            InventoryManagerScript.instance.party = LoadPartyMembers(_data.party);

            DayNightCycleScript.instance.currentDate = _data.currentDate;
            InventoryManagerScript.instance.player.GetComponent<PlayerMovement>().Teleport(new Vector2(_data.playerPosX, _data.playerPosY), true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Vector3 position = InventoryManagerScript.instance.player.transform.position;
            _data = new SaveData(MainDialogueManager.Instance.quests.Select(i => i.index).ToList(), GetActiveQuests(),
                GetInventory(), GetEquippedItems(), InventoryManagerScript.instance.skills, 
                GetPartyMembers(), DayNightCycleScript.instance.currentDate, 
                position.x, position.y, MainDialogueManager.Instance.finishedQuests.Select(i => i.index).ToList());
            
            SaveManager.Save(Application.persistentDataPath + "/savedata.zues", _data);    
        }
    }

    private static int[] GetEquippedItems()
    {
        return InventoryManagerScript.instance.equippedItems.Select(i => i==null ? -1 : i.index).ToArray();
    }

    private static List<ItemInInventory> LoadInventory(IEnumerable<ItemInInventoryData> dta)
    {
        return (from itemInInventoryData in dta from instanceAllItem in InventoryManagerScript.instance.allItems where itemInInventoryData.index == instanceAllItem.index select new ItemInInventory(instanceAllItem, itemInInventoryData.equipped)).ToList();
    }

    private static InventoryItem[] LoadEquipped(IEnumerable<int> arr)
    {
        //return (from i in arr from ii in InventoryManagerScript.instance.allItems where ii.index == i select ii).ToArray();
        List<InventoryItem> l = new List<InventoryItem>();
        foreach (int i in arr)
        {
            foreach (InventoryItem instanceAllItem in InventoryManagerScript.instance.allItems)
            {
                if (i < 0)
                {
                    l.Add(null);
                    break;
                }
                else if (instanceAllItem.index == i)
                {
                    l.Add(instanceAllItem);
                    break;
                }
            }
        }

        return l.ToArray();
    }
    
    private static List<ItemInInventoryData> GetInventory()
    {
        return InventoryManagerScript.instance.inventory.Select(itemInInventory => new ItemInInventoryData(itemInInventory.item.index, itemInInventory.equipped)).ToList();
    }

    private static List<ActiveQuestData> GetActiveQuests()
    {
        return MainDialogueManager.Instance.activeQuests.Select(instanceActiveQuest => new ActiveQuestData(instanceActiveQuest.quest.index, instanceActiveQuest.pos)).ToList();
    }
    private static List<ActiveQuest> LoadActiveQuests(IEnumerable<ActiveQuestData> dta)
    {
        return (from activeQuestData in dta from quest in MainDialogueManager.Instance.allQuests where activeQuestData.index == quest.index select new ActiveQuest(quest, activeQuestData.pos)).ToList();
    }
    
    private static List<Quest> LoadQuests(IEnumerable<int> q)
    {
        return (from i in q from quest in MainDialogueManager.Instance.allQuests where i == quest.index select quest).ToList();
    }

    private List<PartyMemberData> GetPartyMembers()
    {
        return InventoryManagerScript.instance.party.Select(partyMember => new PartyMemberData(partyMember.speaker.index, partyMember.health, partyMember.baseSkills, partyMember.mainHand == null ? -1 : partyMember.mainHand.index, partyMember.offHand == null ? -1 : partyMember.offHand.index, partyMember.criticalFailPercentage)).ToList();
    }
    
    private static List<PartyMember> LoadPartyMembers(IEnumerable<PartyMemberData> q)
    {
        List<PartyMember> l = new List<PartyMember>();
        foreach (PartyMemberData partyMemberData in q)
        {
            InventoryItem mh = null;
            InventoryItem oh = null;

            Speaker s = InventoryManagerScript.instance.allSpeakers.FirstOrDefault(instanceAllSpeaker => instanceAllSpeaker.index == partyMemberData.speakerId);

            foreach (InventoryItem instanceAllItem in InventoryManagerScript.instance.allItems)
            {
                if (instanceAllItem.index == partyMemberData.mainHand)
                {
                    mh = instanceAllItem;
                }

                if (instanceAllItem.index == partyMemberData.offHand)
                {
                    oh = instanceAllItem;
                }
            }
            
            l.Add(new PartyMember(s, partyMemberData.health, partyMemberData.baseSkills, mh, oh, partyMemberData.criticalFailPercentage));
        }

        return l;
    }
}
