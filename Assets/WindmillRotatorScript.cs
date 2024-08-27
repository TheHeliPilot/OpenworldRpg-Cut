using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WindmillRotatorScript : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private float _angle;

    private void FixedUpdate()
    {
        _angle += speed * Time.fixedDeltaTime;
        transform.rotation = quaternion.Euler(0, 0, _angle);
        if (_angle > 1000000)
            _angle = 0;
    }
}
