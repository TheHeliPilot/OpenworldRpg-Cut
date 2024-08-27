using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float time;

    private void Start()
    {
        Destroy(gameObject, time);
    }
}
