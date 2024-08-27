using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchodyScript : MonoBehaviour
{
    public Transform tpPos;
    public GameObject thisFloor;
    public GameObject nextFloor;
    public bool canTp = true;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!canTp) return;
            nextFloor.SetActive(true);
            thisFloor.SetActive(false);
            //tpPos.gameObject.SetActive(true);
            //gameObject.SetActive(false);
            tpPos.gameObject.GetComponent<SchodyScript>().canTp = false;
            other.transform.position = tpPos.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTp = true;
        }
    }
}
