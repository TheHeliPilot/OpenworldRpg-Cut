using System;
using ScriptableObjects;
using UnityEngine;

[Serializable]
public class StatPair
{
    public Skills skill;
    public float value;
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class InventoryItem : ScriptableObject
{
    public int index;
    
    [SerializeField]
    private string _name;

    [TextArea]
    public string description;
    public AttackAction[] actions;
    public ItemSlots[] slots;
    public int armorValue = -1;
    public ClothesPair clothes;
    public Sprite itemImage;
    public StatPair[] statBonuses;
    public StatPair[] statRequirements;
    
    public string GetName()
    {
        return _name;
    }
}