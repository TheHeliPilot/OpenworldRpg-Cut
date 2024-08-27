using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderOffScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetComponent<EdgeCollider2D>().enabled = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            GetComponent<EdgeCollider2D>().enabled = true;
    }
}
