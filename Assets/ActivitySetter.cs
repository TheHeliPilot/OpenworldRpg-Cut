using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivitySetter : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(target.activeSelf);
        }
    }
}
