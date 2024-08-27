using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public GameObject map;
    public RectTransform player;

    private void Update()
    {
        Vector3 position = InventoryManagerScript.instance.player.transform.position;
        player.anchoredPosition =
            new Vector2(position.x * (700f / 5900), position.y * (520f / 5900));
    }
}
