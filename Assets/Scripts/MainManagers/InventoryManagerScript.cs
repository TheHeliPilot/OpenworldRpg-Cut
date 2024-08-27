using System;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using ScriptableObjects;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Other
{
    [Serializable]
    public class PartyMember
    {
        public PartyMember(Speaker speaker, int[] health, StatPair[] skills, InventoryItem mainHand, InventoryItem offHand, int criticalFailChance)
        {
            this.speaker = speaker;
            this.health = new int[health.Length];
            maxHealth = new int[health.Length];
            for (int i = 0; i < health.Length; i++)
            {
                this.health[i] = health[i];
                maxHealth[i] = health[i];
            }
            baseSkills = skills;
            if (mainHand != null)
                this.mainHand = mainHand;
            if (offHand != null)
                this.offHand = offHand;
            stamina = 20;
            criticalFailPercentage = criticalFailChance;
        }
    
        public Speaker speaker;
        public int[] health = new int[5];
        public int[] maxHealth = new int[5];
        public StatPair[] baseSkills = new StatPair[6];
        public InventoryItem mainHand;
        public InventoryItem offHand;
        public bool canAct;
        public bool isDead;
        public List<InFightState> states = new();
        public int stamina;
        public int criticalFailPercentage;
    }

    [Serializable]
    public class ItemInInventory
    {
        public ItemInInventory(InventoryItem i)
        {
            item = i;
        }
        public ItemInInventory(InventoryItem i, bool equipped)
        {
            item = i;
            this.equipped = equipped;
        }
        public InventoryItem item;
        public bool equipped;
    }

    public class InventoryManagerScript : MonoBehaviour
    {
        public static InventoryManagerScript instance;

        public InventoryItem[] allItems;
        public Speaker[] allSpeakers;

        public Volume pp;
        public GameObject player;
        public PartyMemberScriptableObject playerPartyMember;
        public GameObject inventoryMenu;
        public GameObject spawnableItemObject;
        public List<ItemInInventory> inventory;
        public List<ItemInInventory> inventoryOther;
        public TMP_Text itemDescriptionText;
        public TMP_Text itemDescriptionTextOther;
        public GameObject useButton;
        public GameObject equipButton;
        public GameObject discardButton;
        public GameObject takeButton;
        public GameObject stowButton;
        public GameObject otherInventory;
        public SlideInOutButton[] partyMemberButtons;
        public AudioCue paperRustle;
    
        // head, torso, legs, armor, additional, weapon
        [NamedArray (new string[] {"Head", "Torso", "Legs", "MainHand", "OffHand", "Neck", "Back", "Ring"})]
        public InventoryItem[] equippedItems = new InventoryItem[8];
        //[NamedArrayAttribute (new string[] {"Str", "Dex", "Con", "Int", "Wis", "Cha"})]
        public float[] skills = new float[6];
    
        public List<PartyMember> party;

        [SerializeField] private GameObject inventoryObject;
        [SerializeField] private RectTransform inventoryContent;
        [SerializeField] private RectTransform inventoryContentOther;
        private readonly List<GameObject> _inventoryButtonObjects = new();
        private readonly List<GameObject> _inventoryButtonObjectsOther = new();

        private ItemInInventory _selectedItem;
        private ItemInInventory _selectedItemOther;
        [HideInInspector] public bool isInInventoryOther;
    
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            //DontDestroyOnLoad(gameObject);
        } 

        private void Start()
        {
            //InvokeRepeating(nameof(UpdateInventory), 0, .1f); 
            UpdateInventory();
            party[0].states = new List<InFightState>();
            //AddPartyMember(playerPartyMember);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !TurnBasedAlternativeManager.turnBasedAlternativeInstance.isInCombat &&
                !TutorialManager.instance.isInTutorial)
            {
                TooltipSystem.HideTooltip();
               OpenInventory();
            }

            if (_selectedItem == null)
            {
                useButton.SetActive(false);
                equipButton.SetActive(false);
                discardButton.SetActive(false);
            }
            else
            {
                useButton.SetActive(true);
                discardButton.SetActive(true);
            
                if (_selectedItem?.item.slots.Length > 0)
                    equipButton.SetActive(true);
            }
            
            takeButton.SetActive(_selectedItemOther != null);
            stowButton.SetActive(_selectedItem != null);
            
            if(isInInventoryOther)
                equipButton.SetActive(false);
            else
                stowButton.SetActive(false);
        
            for (int i = 0; i < party.Count; i++)
            {
                partyMemberButtons[i].SetFunc(party[i].canAct);
                //partyMemberButtons[i].SetFunc(party[i].canAct);
            }
            
            if(otherInventory.activeSelf != isInInventoryOther)
                otherInventory.SetActive(isInInventoryOther);
        }

        public bool forceInventory;
        public void OpenInventory()
        {
            if(!TurnBasedAlternativeManager.turnBasedAlternativeInstance.isInCombat) TurnBasedAlternativeManager.turnBasedAlternativeInstance.ClickInventoryButton();
            forceInventory = false;
            MainAudioManager.instance.SpawnAudio(paperRustle.GetSound(), Camera.main.transform.position, paperRustle.volume, 150, false, false);
            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
            if (!isInInventoryOther && inventoryMenu.activeSelf)
                forceInventory = true;
            isInInventoryOther = false;
            
            if (_otherInventoryObject != null)
            {
                _otherInventoryObject.items.Clear();
                foreach (ItemInInventory itemInInventory in inventoryOther)
                {
                    _otherInventoryObject.items.Add(itemInInventory.item);
                }
            }
        }
        
        public void AddPartyMember(PartyMemberScriptableObject member)
        {
            if(party.Count < 3)
                party.Add(new PartyMember(member.speaker, member.health, member.baseSkills, member.mainHand, member.offHand, member.criticalFailPercentage));
        }

        public void RemovePartyMember(PartyMemberScriptableObject member)
        {
            foreach (PartyMember p in party.Where(p => p.speaker == member.speaker))
            {
                party.Remove(p);
                return;
            }
        }

        private void UpdateInventory()
        {
            for (int i = 0; i < party[0].baseSkills.Length; i++)
            {
                //Debug.Log(i);
                float stat = party[0].baseSkills[i].value;
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (InventoryItem item in equippedItems)
                {
                    if(item == null)
                        continue;
                    if(item.statBonuses.Length == 0)
                        continue;
                
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (StatPair pair in item.statBonuses)
                    {
                        if (pair.skill == party[0].baseSkills[i].skill)
                            stat += pair.value;
                    }
                }
                skills[i] = stat;
            }
        
            foreach (GameObject o in _inventoryButtonObjects)
            {
                Destroy(o);
            }
            //inventoryContent.sizeDelta = new Vector2(0, inventory.Count * 26);
            foreach (ItemInInventory i in inventory)
            {
                GameObject g = Instantiate(inventoryObject, inventoryContent.transform);
                _inventoryButtonObjects.Add(g);
                g.GetComponent<InventoryItemScript>().Setup(i);
            }
            foreach (GameObject o in _inventoryButtonObjectsOther)
            {
                Destroy(o);
            }
            foreach (ItemInInventory i in inventoryOther)
            {
                GameObject g = Instantiate(inventoryObject, inventoryContentOther.transform);
                _inventoryButtonObjectsOther.Add(g);
                g.GetComponent<InventoryItemScript>().Setup(i, true);
            }
        }
    
        public void AddItem(InventoryItem item)
        {
            ItemInInventory n = new ItemInInventory(item);
            inventory.Add(n);
            UpdateInventory();
        }
    
        public void RemoveItem(InventoryItem item)
        {
            foreach (ItemInInventory itemInInventory in inventory.Where(itemInInventory => itemInInventory.item == item && !itemInInventory.equipped))
            {
                inventory.Remove(itemInInventory);
                break;
            }
            for (int i = 0; i < equippedItems.Length; i++)
            {
                if (equippedItems[i] != item) continue;
                equippedItems[i] = null;
                return;
            }
            UpdateInventory();
        }

        public bool HasItem(InventoryItem item)
        {
            return inventory.Any(i => i.item == item);
        }

        public int GetItemAmount(InventoryItem item)
        {
            return inventory.Count(inventoryItem => inventoryItem.item == item);
        }

        public void SpawnItem(ItemInInventory item, Vector3 position)
        {
            GameObject g = Instantiate(spawnableItemObject, position, quaternion.identity);
            g.GetComponent<ItemScript>().item = item.item;
        }

        public void SelectItem(ItemInInventory item)
        {
            _selectedItem = item;
            itemDescriptionText.text = item.item.description;
            //Debug.Log($"selected item {item.GetName()}");
        }
        public void SelectItemOther(ItemInInventory item)
        {
            _selectedItemOther = item;
            itemDescriptionTextOther.text = item.item.description;
            //Debug.Log($"selected item {item.GetName()}");
        }

        public void UseSelectedItem()
        {
            Debug.Log($"Used {_selectedItem.item.GetName()}");
        }

        public void EquipSelectedItem(int slotIndex)
        {
            bool wasSelected = false;
        
            if(_selectedItem.equipped)
                return;
        
            foreach (ItemSlots slot in _selectedItem.item.slots)
            {
                switch (slotIndex)
                {
                    case 0:
                        if (slot == ItemSlots.head)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }

                        break;
                    case 1:
                        if (slot == ItemSlots.torso)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                    case 2:
                        if (slot == ItemSlots.legs)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                    case 3:
                        if (slot == ItemSlots.mainHand)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                    case 4:
                        if (slot == ItemSlots.offHand)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                    case 5:
                        if (slot == ItemSlots.neck)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                    case 6:
                        if (slot == ItemSlots.back)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                    case 7:
                        if (slot == ItemSlots.ring)
                        {
                            equippedItems[slotIndex] = _selectedItem.item;
                            wasSelected = true;
                        }
                        break;
                }
            }

            if (!wasSelected)
            {
                Debug.LogWarning($"Item {_selectedItem.item.GetName()} is not suitable for selected slot!");
                TooltipSystem.ShowPopoutTooltip($"Item {_selectedItem.item.GetName()} is not suitable for selected slot!");
            }
            else
                _selectedItem.equipped = true;
        }

        public void UnequipItem(int slotIndex)
        {
            foreach (ItemInInventory itemInInventory in inventory.Where(itemInInventory => itemInInventory.item == equippedItems[slotIndex] && itemInInventory.equipped))
            {
                itemInInventory.equipped = false;
            }
            equippedItems[slotIndex] = null;
        }

        public void DiscardSelectedItem()
        {
            if(_selectedItem.equipped) return;
            RemoveItem(_selectedItem.item);
            _selectedItem = null;
            itemDescriptionText.text = "";
            UpdateInventory();
        }

        public void MoveItem()
        {
            if(_selectedItemOther == null) return;
            
            inventoryOther.Remove(_selectedItemOther);
            AddItem(_selectedItemOther.item);
        }

        public void TakeAll()
        {
            foreach (ItemInInventory itemInInventory in inventoryOther)
            {
                inventory.Add(itemInInventory);
            }
            inventoryOther.Clear();
            UpdateInventory();
        }

        private InventoryObjectScript _otherInventoryObject;
        public void SetOtherInventory(List<InventoryItem> items, InventoryObjectScript otherInventory)
        {
            _otherInventoryObject = otherInventory;
            inventoryOther.Clear();
            isInInventoryOther = true;
            foreach (ItemInInventory n in items.Select(inventoryItem => new ItemInInventory(inventoryItem)))
            {
                inventoryOther.Add(n);
            }
            UpdateInventory();
        }

        public void StowItem()
        {
            if (_selectedItem.equipped)
            {
                TooltipSystem.ShowPopoutTooltip("Item equipped!");
                return;
            }
            inventoryOther.Add(_selectedItem);
            RemoveItem(_selectedItem.item);
            _selectedItem = null;
            UpdateInventory();
        }

        public int GetSkill(Skills skill)
        {
            switch (skill)
            {
                default:
                    return -1;
            }
        }
    }
}