using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MainManagers;
using Other;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Other.InventoryManagerScript;

public class ActionSectionScript : MonoBehaviour
{
    public Color normalColor;
    public Color selectedColor;
    
    public int currentlySelectedPart;
    
    [NamedArrayAttribute (new string[] {"Head", "Torso", "Legs", "MainHand", "OffHand"})]
    public Image[] bodyParts;
    [NamedArrayAttribute (new string[] {"Head", "Torso", "Legs", "MainHand", "OffHand"})]
    public Image[] hitBodyParts;
    public Image[] partyMemberImages;
    public Image[] enemyImages;
    public GameObject[] bleeding;
    public AttackAction punch;

    public Toggle offhand;
    
    [Header("Texts")]
    public TMP_Text bodyPartText;
    public TMP_Text bodyPartStatus;
    public TMP_Text bodyPartEquipment;
    public TMP_Text bodyPartHitChance;
    public TMP_Text currentWeapon;
    
    public GameObject weaponActionButton;
    public Transform weaponActions;

    [Header("Statuses")]
    public GameObject statusContentEnemy;
    public GameObject statusContentPlayer1;
    public GameObject statusContentPlayer2;
    public GameObject statusContentPlayer3;
    [Header("Resistances")]
    public GameObject resistancesContent;
    public GameObject statusCircle;
    [Tooltip("buldgeoning, piercing, slashing, fire, cold, necrotic, holy, acid")]
    public Sprite[] resistanceImages;
    [Header("Stamina")]
    public TMP_Text staminaTextP1;
    public TMP_Text staminaTextP2;
    public TMP_Text staminaTextP3;
    public Slider staminaSliderP1;
    public Slider staminaSliderP2;
    public Slider staminaSliderP3;

    private InventoryItem _currentWeaponEquipped;
    
    private void Start()
    {
        InvokeRepeating(nameof(CheckWeapons), 0, .2f);
        //InvokeRepeating(nameof(UpdateBleed), 0, 1);
    }
    
    private void Update()
    {
        foreach (FightingEnemy fightingEnemy in TurnBasedAlternativeManager.turnBasedAlternativeInstance.currentEnemies
                     .Where(fightingEnemy => fightingEnemy.pos == _selectedEnemy))
        {
            bodyPartEquipment.text = fightingEnemy.character.equipment[currentlySelectedPart];
            
            bodyPartStatus.text = "Normal";
            if (fightingEnemy.health[currentlySelectedPart] < fightingEnemy.character.health[currentlySelectedPart] / 2)
                bodyPartStatus.text = "Bleeding";
            if (fightingEnemy.health[currentlySelectedPart] <= 0)
                bodyPartStatus.text = "Destroyed";

            for (int i = 0; i < 5; i++)
            {
                //bodyParts[currentlySelectedPart].color = new Color(226, 190, 104, 255);

                if (fightingEnemy.health[i] <=
                    (float)fightingEnemy.character.health[i] / 2)
                {
                    if(!bleeding[i].activeSelf) bleeding[i].SetActive(true);
                }
                else
                {
                    if(bleeding[i].activeSelf) bleeding[i].SetActive(false);
                }
                
                bodyPartHitChance.text = fightingEnemy.character.hitChancePercent[currentlySelectedPart].ToString();
                //break;
            }
        }
        
        foreach (Image spriteRenderer in bodyParts)
        {
            spriteRenderer.color = normalColor;
        }

        for (int i = 0; i < hitBodyParts.Length; i++)
        {
            hitBodyParts[i].fillAmount = 1 - 1f / instance.party[_selectedPartyMember].maxHealth[i] * instance.party[_selectedPartyMember].health[i];
        }
        
        bodyParts[currentlySelectedPart].color = selectedColor;

        bodyPartText.text = currentlySelectedPart switch
        {
            0 => "Head",
            1 => "Torso",
            2 => "Legs",
            3 => "Main Hand",
            4 => "Off Hand",
            _ => "problem"
        };

        if (TurnBasedAlternativeManager.turnBasedAlternativeInstance.isInCombat)
        {
            partyMemberImages[0].sprite = _selectedPartyMember == 0 ? instance.party[0].speaker.GetSpriteOutline() : instance.party[0].speaker.GetSprite();
            partyMemberImages[0].color = instance.party[0].isDead ? Color.gray : Color.white;
            staminaTextP1.text = InventoryManagerScript.instance.party[0].stamina + "/20";
            staminaSliderP1.value = Mathf.Lerp(staminaSliderP1.value, Mathf.Clamp(InventoryManagerScript.instance.party[0].stamina, 0, 20), Time.deltaTime * 10);
            
            if (instance.party.Count > 1)
            {
                partyMemberImages[1].sprite = _selectedPartyMember == 1 ? instance.party[1].speaker.GetSpriteOutline() : instance.party[1].speaker.GetSprite();
                partyMemberImages[1].color = instance.party[1].isDead ? Color.gray : Color.white;
                partyMemberImages[1].GetComponentInChildren<Button>().interactable = true;
                staminaTextP2.gameObject.SetActive(true);
                staminaSliderP2.gameObject.SetActive(true);
                staminaTextP2.text = InventoryManagerScript.instance.party[1].stamina + "/20";
                staminaSliderP2.value = Mathf.Lerp(staminaSliderP2.value, Mathf.Clamp(InventoryManagerScript.instance.party[1].stamina, 0, 20), Time.deltaTime * 10);
            }
            else
            {
                partyMemberImages[1].GetComponentInChildren<Button>().interactable = false;
                partyMemberImages[1].color = Color.clear;
                staminaTextP2.gameObject.SetActive(false);
                staminaSliderP2.gameObject.SetActive(false);
            }

            if (instance.party.Count > 2)
            {
                partyMemberImages[2].sprite = _selectedPartyMember == 2 ? instance.party[2].speaker.GetSpriteOutline() : instance.party[2].speaker.GetSprite();
                partyMemberImages[2].color = instance.party[2].isDead ? Color.gray : Color.white;
                partyMemberImages[2].GetComponentInChildren<Button>().interactable = true;
                staminaTextP3.gameObject.SetActive(true);
                staminaSliderP3.gameObject.SetActive(true);
                staminaTextP3.text = InventoryManagerScript.instance.party[2].stamina + "/20";
                staminaSliderP3.value = Mathf.Lerp(staminaSliderP3.value, Mathf.Clamp(InventoryManagerScript.instance.party[2].stamina, 0, 20), Time.deltaTime * 10);
            }
            else
            {
                partyMemberImages[2].GetComponentInChildren<Button>().interactable = false;
                partyMemberImages[2].color = Color.clear;
                staminaTextP3.gameObject.SetActive(false);
                staminaSliderP3.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Image partyMemberImage in partyMemberImages)
            {
                partyMemberImage.color = Color.clear;
            }
        }

        
    }

    private InventoryItem _previousWeapon;
    private bool _isEmpty;
    public void CheckWeapons()
    {
        if (_selectedPartyMember == 0)
        {
            _currentWeaponEquipped = offhand.isOn
                ? instance.equippedItems[4]
                : instance.equippedItems[3];
        }
        else
        {
            _currentWeaponEquipped = offhand.isOn
                ? instance.party[_selectedPartyMember].offHand
                : instance.party[_selectedPartyMember].mainHand;
        }

        if (_currentWeaponEquipped == null)
        {
            if (_isEmpty != true)
            {
               foreach (Transform t in weaponActions)
                {
                    Destroy(t.gameObject);
                    currentWeapon.text = "No weapon equipped!";
                }


                UpdateWeaponActions(true);
                _isEmpty = true;
            }
        }
        else
        {
            if (_previousWeapon != _currentWeaponEquipped)
            {
                UpdateWeaponActions();
                currentWeapon.text = _currentWeaponEquipped.GetName();
                _isEmpty = false;
            }
        }

        _previousWeapon = _currentWeaponEquipped;
    }
    
    private void UpdateWeaponActions(bool empty = false)
    {
        foreach (Transform t in weaponActions)
        {
            Destroy(t.gameObject);
        }

        if (empty)
        {
            GameObject g = Instantiate(weaponActionButton, weaponActions);
            g.GetComponent<ItemActionButtonScript>().Setup(punch, this);
            return;
        }
        
        foreach (AttackAction vaAction in _currentWeaponEquipped.actions)
        {
            GameObject g = Instantiate(weaponActionButton, weaponActions);
            g.GetComponent<ItemActionButtonScript>().Setup(vaAction, this);
        }
    }
    
    public void BodypartSelect(int i)
    {
        currentlySelectedPart = i;
    }

    private int _selectedEnemy;
    public void SelectEnemy(int enemy)
    {
        _selectedEnemy = enemy;
        TurnBasedAlternativeManager.turnBasedAlternativeInstance.SelectEnemy(enemy);
        TurnBasedAlternativeManager.turnBasedAlternativeInstance.UpdateEnemies();
        UpdateBleed();
        
        UpdateStatusResistance();
    }

    public void UpdateStatusResistance()
    {
        foreach (Transform g in resistancesContent.transform)
        {
            Destroy(g.gameObject);
        }
        foreach (Transform g in statusContentEnemy.transform)
        {
            Destroy(g.gameObject);
        }
        foreach (Transform t in statusContentPlayer1.transform)
        {  
            Destroy(t.gameObject);
        }
        foreach (Transform t in statusContentPlayer2.transform)
        {  
            Destroy(t.gameObject);
        }
        foreach (Transform t in statusContentPlayer3.transform)
        {  
            Destroy(t.gameObject);
        }

        foreach (FightingEnemy fightingEnemy in TurnBasedAlternativeManager.turnBasedAlternativeInstance.currentEnemies
                     .Where(fightingEnemy => fightingEnemy.pos == _selectedEnemy))
        {
            //Debug.Log(fightingEnemy);
            
            foreach (DamagePair dp in fightingEnemy.resistances)
            {
                GameObject g = Instantiate(statusCircle, resistancesContent.transform);
                //buldgeoning, piercing, slashing, fire, cold, necrotic, holy, acid
                switch (dp.type)
                {
                    case DamageType.buldgeoning:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[0];
                        break;
                    case DamageType.piercing:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[1];
                        break;
                    case DamageType.slashing:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[2];
                        break;
                    case DamageType.fire:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[3];
                        break;
                    case DamageType.cold:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[4];
                        break;
                    case DamageType.necrotic:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[5];
                        break;
                    case DamageType.holy:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[6];
                        break;
                    case DamageType.acid:
                        g.transform.GetChild(1).GetComponent<Image>().sprite = resistanceImages[7];
                        break;
                }
                //g.GetComponentInChildren<TMP_Text>().text = char.ToUpper(dp.type.ToString()[0]).ToString();
                g.GetComponent<TooltipHandler>().content = dp.type.ToString();
            }
            
            foreach (InFightState st in fightingEnemy.states){
                GameObject g = Instantiate(statusCircle, statusContentEnemy.transform);
                g.transform.GetChild(1).GetComponent<Image>().sprite = st.state.image;
            }
        }
        
        for (int i = 0; i < instance.party.Count; i++)
        {
            foreach (InFightState state in instance.party[i].states)
            {
                GameObject g;
                switch (i)
                {
                    case 0:
                        g = Instantiate(statusCircle, statusContentPlayer1.transform);
                        g.transform.GetChild(1).GetComponent<Image>().sprite = state.state.image;
                        break;
                    case 1:
                        g = Instantiate(statusCircle, statusContentPlayer2.transform);
                        g.transform.GetChild(1).GetComponent<Image>().sprite = state.state.image;
                        break;
                    case 2:
                        g = Instantiate(statusCircle, statusContentPlayer3.transform);
                        g.transform.GetChild(1).GetComponent<Image>().sprite = state.state.image;
                        break;
                }
            }
        }
    }

    private void UpdateBleed()
    {
        foreach (GameObject o in bleeding)
        {
            o.SetActive(false); 
        }
    }
    
    private int _selectedPartyMember;
    public void SelectPartyMember(int i)
    {
        _isEmpty = false;
        _selectedPartyMember = i;
    }

    public void DoAction(AttackAction action)
    {
        if (!instance.party[_selectedPartyMember].canAct)
        {
            Debug.LogWarning("Cannot Act!");
            TooltipSystem.ShowPopoutTooltip(
                instance.party[_selectedPartyMember].states.Any(s => s.state.stun)
                    ? $"{instance.party[_selectedPartyMember].speaker.GetName()} is stunned!"
                    : $"{instance.party[_selectedPartyMember].speaker.GetName()} is out of actions!");

            return;
        }
        TurnBasedAlternativeManager.turnBasedAlternativeInstance.DoAction(action, _selectedEnemy, currentlySelectedPart, _selectedPartyMember);
    }
}
