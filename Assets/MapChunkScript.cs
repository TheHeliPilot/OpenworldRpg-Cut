using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunkScript : MonoBehaviour
{
    public string areaName;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("PlayerTrigger") && areaName != "")
            TooltipSystem.ShowAreaNameTooltip(areaName);
    }
}
