using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPoolChecker : MonoBehaviour
{
    public int type;

    private GameObject _pooled;
    
    private void Start()
    {
        //InvokeRepeating(nameof(Pool), Random.Range(0f, 2f), Random.Range(0.5f, 1f));
    }

    public void Pool()
    {
        if (_pooled != null)
        {
            if (_pooled.transform.position == transform.position)
                return;
        }
        
        switch (type)
        {
            case 0:
                ObjectPoolManager.instance.PoolTree(transform.position, this);
                break;
            case 1:
                ObjectPoolManager.instance.PoolBush(transform.position, this);
                break;
        }
    }

    public void SetPooled(GameObject g)
    {
        _pooled = g;
    }
}
