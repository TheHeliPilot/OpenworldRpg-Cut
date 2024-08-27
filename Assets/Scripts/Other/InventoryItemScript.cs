using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemScript : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image image;
    [SerializeField] private GameObject equipped;
    private bool _isOther;

    private ItemInInventory _item;
    public void Setup(ItemInInventory item, bool isOther = false)
    {
        _isOther = isOther;
        _item = item;
        nameText.text = item.item.GetName();
        image.sprite = item.item.itemImage;
    }

    private void Update()
    {
        equipped.SetActive(_item.equipped);
    }

    public void Click()
    {
        if (!_isOther)
            InventoryManagerScript.instance.SelectItem(_item);
        else
            InventoryManagerScript.instance.SelectItemOther(_item);
    }
}
