using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunkSpawnerScript : MonoBehaviour
{
    public GameObject chunk;
    public Vector2 size;
    
    private void Start()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GameObject g = Instantiate(chunk, new Vector3(i * 200, j * 200, 0), Quaternion.identity);
                g.SetActive(false);
            }
        }
    }
}
