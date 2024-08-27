using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ImageAnimPlayer : MonoBehaviour
{
    public Sprite[] animSprites;
    public float fps;
    public int start;
    public bool loop = true;

    private int _animPart;
    private float _time;

    private void Start()
    {
        _animPart = start;
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!(_time >= 1 / fps)) return;
        GetComponent<Image>().sprite = animSprites[_animPart];
        _animPart += Mathf.RoundToInt(_time / (1 / fps));
        _time = 0;
        if (_animPart >= animSprites.Length)
        {
            if(loop)
                _animPart = 0;
            else
                Destroy(gameObject);
        }
    }
}
