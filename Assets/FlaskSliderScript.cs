using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlaskSliderScript : MonoBehaviour
{
    public GameObject drip;

    private void Update()
    {
        drip.SetActive(GetComponent<Slider>().value > GetComponent<Slider>().maxValue * .9f);
    }
}
