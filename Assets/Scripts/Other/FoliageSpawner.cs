using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PolygonCollider2D))]
public class FoliageSpawner : MonoBehaviour
{
    public List<GameObject> prefabs;
    public int count;
    public bool randomSize;
    public bool randomFlip;
    
    private PolygonCollider2D _pc;

    private void Start()
    {
        _pc = GetComponent<PolygonCollider2D>();

        for (int i = 0; i < count; i++)
        {
            GameObject g = Instantiate(prefabs[Random.Range(0, prefabs.Count)], RandomPointInCollider.GetPointInsideCollider(_pc), quaternion.identity);
            g.transform.SetParent(transform);
            float scale = Random.Range(.8f, 1f);
            if (randomSize)
                g.transform.localScale = new Vector3(scale, scale, 1);
            
            if (!randomFlip) continue;
            foreach (SpriteRenderer sr in g.GetComponentsInChildren(typeof(SpriteRenderer)))
            {
                if (!sr.gameObject.CompareTag("Shadow"))
                    sr.flipX = !(Random.value > .5f);
            }
            if(g.gameObject.GetComponent<FoliageToggler>() != null)
               g.gameObject.GetComponent<FoliageToggler>().Toggle(false);
        }
    }
}
