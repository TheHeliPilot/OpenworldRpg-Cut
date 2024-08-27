using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using UnityEngine;

public class InventoryObjectScript : MonoBehaviour
{
    public List<InventoryItem> items;

    public Material normal;
    public Material selected;

    public void OpenInventory()
    {
        if(InventoryManagerScript.instance.isInInventoryOther || InventoryManagerScript.instance.forceInventory) return;
        
        InventoryManagerScript.instance.SetOtherInventory(items, this);
        if(!InventoryManagerScript.instance.inventoryMenu.activeSelf)
            InventoryManagerScript.instance.OpenInventory();
    }

    public void PlayerEnter()
    {
        GetComponent<SpriteRenderer>().material = selected;
    }

    public void PlayerExit()
    {
        GetComponent<SpriteRenderer>().material = normal;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.isTrigger) return;
        other.GetComponent<PlayerMovement>()._inventoryObjects.Add(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || other.isTrigger) return;
        other.GetComponent<PlayerMovement>()._inventoryObjects.Remove(this);
        if (other.GetComponent<PlayerMovement>().io == this) other.GetComponent<PlayerMovement>().io = null;
        PlayerExit();
    }
}
