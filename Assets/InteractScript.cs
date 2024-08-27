using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public void AnimalBurn()
    {
        if(GetComponent<AnimalWalkScript>() == null) return;
        
        GetComponent<AnimalWalkScript>().BURN();
    }
}
