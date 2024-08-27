using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class FoliageToggler : MonoBehaviour
{
    public bool onlyFirstChild = false;
    public void Toggle(bool state)
    {
        //gameObject.SetActive(state);
        
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().enabled = state;

        if (onlyFirstChild)
        {
            transform.GetChild(0).gameObject.SetActive(state);    
            return;
        }
        
        foreach (Transform o in transform)
        {
            o.gameObject.SetActive(state);
        }
    }
}
