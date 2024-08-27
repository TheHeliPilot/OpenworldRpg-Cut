using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCamCanvas : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
