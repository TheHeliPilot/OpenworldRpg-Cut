using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public InventoryItem item;
    public bool canPickup = true;

    private void Start()
    {
        Invoke(nameof(Begin), .5f);
    }

    private void Begin()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
    }
}
