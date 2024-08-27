using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light2D))]
public class FireFlicker : MonoBehaviour
{
    public Gradient colors;
    public float intensityMin;
    public float intensityMax;

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            //GetComponent<Light2D>().color = colors.Evaluate(Random.Range(0f, 1f));
            float desiredIntensity = Random.Range(intensityMin, intensityMax);
            if (desiredIntensity > GetComponent<Light2D>().intensity)
                GetComponent<Light2D>().intensity += .1f;
            else
                GetComponent<Light2D>().intensity -= .1f;
            yield return new WaitForSeconds(.05f);
        }
    }
}
