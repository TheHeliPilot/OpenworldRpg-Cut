using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;
    [SerializeField] private GameObject[] _treePool = new GameObject[100];
    [SerializeField] private GameObject[] _bushPool = new GameObject[100];

    public GameObject[] trees;
    public GameObject[] bushes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _treePool.Length; i++)
        {
            GameObject g = Instantiate(trees[Random.Range(0, trees.Length)], transform);
            g.transform.position = new Vector3(-1000, -1000);
            _treePool[i] = g;
        }
        for (int i = 0; i < _bushPool.Length; i++)
        {
            GameObject g = Instantiate(bushes[Random.Range(0, bushes.Length)], transform);
            g.transform.position = new Vector3(-1000, -1000);
            _bushPool[i] = g;
        }
    }

    private int _treeCounter;
    private int _bushCounter;

    public void PoolBush(Vector3 pos, ObjectPoolChecker objectPoolChecker)
    {
        _bushPool[_bushCounter].transform.position = pos;
        objectPoolChecker.SetPooled(_bushPool[_bushCounter]);
        _bushCounter++;
        if (_bushCounter >= _bushPool.Length)
        {
            _bushCounter = 0;
        }
    }
    
    public void PoolTree(Vector3 pos, ObjectPoolChecker objectPoolChecker)
    {
        _treePool[_treeCounter].transform.position = pos;
        objectPoolChecker.SetPooled(_treePool[_treeCounter]);
        _treeCounter++;
        if (_treeCounter >= _treePool.Length)
        {
            _treeCounter = 0;
        }
    }
}
