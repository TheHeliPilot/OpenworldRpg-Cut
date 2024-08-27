using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyActionPopupScript : MonoBehaviour
{
    public void Setup(string text)
    {
        GetComponentInChildren<TMP_Text>().text = text;
        Destroy(gameObject, 1.5f);
    }
}
