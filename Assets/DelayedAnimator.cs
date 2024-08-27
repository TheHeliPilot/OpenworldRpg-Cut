using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DelayedAnimator : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Animator>().Play(0,-1, Random.value);
    }
}
