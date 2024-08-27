using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SlideInOutButton : MonoBehaviour
{
    public float yIn;
    public float yOut;
    public float speed;
    public GameObject buttonObject;
    public bool isFunctional { get; private set; }
    public bool startIn;
    public bool overrideOff;
    private bool _lastFunctional;
    
    private float _current;
    
    private void Start()
    {
        _current = startIn ? yIn : yOut;
    }

    private float _lerpVal;
    private void FixedUpdate()
    {
        if (isFunctional != _lastFunctional)
        {
            Set(isFunctional);
            buttonObject.GetComponentInChildren<Button>().interactable = isFunctional;
            //Debug.Log("Functional: " + isFunctional);
        }
        if(overrideOff)
        {
            Set(false);
            buttonObject.GetComponentInChildren<Button>().interactable = false;
        }
        
        _lerpVal += Time.fixedDeltaTime * speed;   
        buttonObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 
            Mathf.Lerp(buttonObject.GetComponent<RectTransform>().anchoredPosition.y, _current, _lerpVal), 0);
        _lastFunctional = isFunctional;
    }

    private float _lastCurrent;
    private void Set(bool on)
    {
        _lastCurrent = _current;
        _current = on ? yOut : yIn;
        
        if(_lastCurrent != _current)
            _lerpVal = 0;
    }

    public void Toggle()
    {
        float d1 = Mathf.Abs(_current - yIn);
        
        Set(d1 < Mathf.Abs(_current - yOut));
    }

    public void SetFunc(bool b)
    {
        isFunctional = b;
        buttonObject.GetComponentInChildren<Button>().interactable = b;
        Set(b);
        //Debug.Log((new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod());
    }
}