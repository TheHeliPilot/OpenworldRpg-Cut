using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class AnimalSpawn
{
    public GameObject animal;
    public int count;
}

public class AnimalSpawner : MonoBehaviour
{
    public AnimalSpawn[] animals;

    private void Start()
    {
        foreach (AnimalSpawn animalSpawn in animals)
        {
            for (int i = 0; i < animalSpawn.count; i++)
            {
                GameObject g = Instantiate(animalSpawn.animal, RandomPointInCollider.GetPointInsideCollider(GetComponent<Collider2D>()), quaternion.identity);
                g.GetComponent<AnimalWalkScript>().bounds = GetComponent<Collider2D>();
            }
        }
    }
}
