using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterShowcaseScript : MonoBehaviour
{
    public Color normalColor;
    public Color selectedColor;
    
    public int currentlySelectedPart;
    
    [NamedArrayAttribute (new string[] {"Head", "Torso", "Legs", "MainHand", "OffHand"})]
    public Image[] bodyParts;
    [NamedArrayAttribute (new string[] {"Head", "Torso", "Legs", "MainHand", "OffHand"})]
    public GameObject[] bodyPartStatus;

    public TMP_Text bodypartText;
    public TMP_Text equipedItemNameText;
    public TMP_Text equipedItemBonusesText;
    public Toggle neckToggle;
    public Toggle backToggle;
    public Toggle ringToggle;
    public GameObject unequipButton;
    
    private void Update()
    {
        foreach (Image spriteRenderer in bodyParts)
        {
            spriteRenderer.color = normalColor;
        }

        foreach (GameObject o in bodyPartStatus)
        {
            o.SetActive(false);
        }

        bodyPartStatus[currentlySelectedPart].SetActive(true);
        bodyParts[currentlySelectedPart].color = selectedColor;

        switch (currentlySelectedPart)
        {
            case 0:
                bodypartText.text = "Head";
                if (neckToggle.isOn)
                {
                    //5
                    equipedItemNameText.text = GetItemName(5);
                    equipedItemBonusesText.text = GetItemStats(5);
                }
                else
                {
                    equipedItemNameText.text = GetItemName(0);
                    equipedItemBonusesText.text = GetItemStats(0);
                }
                break;
            case 1:
                bodypartText.text = "Torso";
                if (backToggle.isOn)
                {
                    //6
                    equipedItemNameText.text = GetItemName(6);
                    equipedItemBonusesText.text = GetItemStats(6);
                }
                else
                {
                    equipedItemNameText.text = GetItemName(1);
                    equipedItemBonusesText.text = GetItemStats(1);
                }
                break;
            case 2:
                bodypartText.text = "Legs";
                equipedItemNameText.text = GetItemName(2);
                equipedItemBonusesText.text = GetItemStats(2);
                break;
            case 3:
                bodypartText.text = "Main Hand";
                if (ringToggle.isOn)
                {
                    //7
                    equipedItemNameText.text = GetItemName(7);
                    equipedItemBonusesText.text = GetItemStats(7);
                }
                else
                {
                    equipedItemNameText.text = GetItemName(3);
                    equipedItemBonusesText.text = GetItemStats(3);
                }
                break;
            case 4:
                bodypartText.text = "Off Hand";
                equipedItemNameText.text = GetItemName(4);
                equipedItemBonusesText.text = GetItemStats(4);
                break;
        }
    }

    private string GetItemName(int slot)
    {
        if (InventoryManagerScript.instance.equippedItems[slot] != null)
        {
            unequipButton.SetActive(true);
            return InventoryManagerScript.instance.equippedItems[slot].GetName();
        }

        unequipButton.SetActive(false);
        return "No item equipped!";
    }
    
    private string GetItemStats(int slot)
    {
        string returnText = "";
        if (InventoryManagerScript.instance.equippedItems[slot] == null) return "";
        
        foreach (StatPair sp in InventoryManagerScript.instance.equippedItems[slot].statBonuses)
        {
            returnText += char.ToUpper(sp.skill.ToString()[0]) + sp.skill.ToString()[1..] + " -> " + sp.value + ", ";
        }

        if (InventoryManagerScript.instance.equippedItems[slot].armorValue > 0)
            returnText += "Armor -> " + InventoryManagerScript.instance.equippedItems[slot].armorValue;
        else if(returnText.Length > 0)
            returnText = returnText.Remove(returnText.Length - 2, 2);

        if (InventoryManagerScript.instance.equippedItems[slot].statBonuses.Length == 0 &&
            InventoryManagerScript.instance.equippedItems[slot].armorValue <= 0){
            returnText = "No bonuses to stats!";}
        
        return returnText;

    }

    public void BodypartSelect(int i)
    {
        currentlySelectedPart = i;
    }

    public void EquipItem()
    {
        switch (currentlySelectedPart)
        {
            case 0:
                InventoryManagerScript.instance.EquipSelectedItem(neckToggle.isOn ? 5 : 0);
                break;
            case 1:
                InventoryManagerScript.instance.EquipSelectedItem(backToggle.isOn ? 6 : 1);
                break;
            case 2:
                InventoryManagerScript.instance.EquipSelectedItem(2);
                break;
            case 3:
                InventoryManagerScript.instance.EquipSelectedItem(ringToggle.isOn ? 7 : 3);
                break;
            case 4:
                InventoryManagerScript.instance.EquipSelectedItem(4);
                break;
        }
    }

    public void UnequipItem()
    {
        switch (currentlySelectedPart)
        {
            case 0:
                InventoryManagerScript.instance.UnequipItem(neckToggle.isOn ? 5 : 0);
                break;
            case 1:
                InventoryManagerScript.instance.UnequipItem(backToggle.isOn ? 6 : 1);
                break;
            case 2:
                InventoryManagerScript.instance.UnequipItem(2);
                break;
            case 3:
                InventoryManagerScript.instance.UnequipItem(ringToggle.isOn ? 7 : 3);
                break;
            case 4:
                InventoryManagerScript.instance.UnequipItem(4);
                break;
        }
    }
}
