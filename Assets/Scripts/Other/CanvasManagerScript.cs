using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManagerScript : MonoBehaviour
{
    public static CanvasManagerScript instance;

    private void Awake()
    {
        instance = this;
    }
}
