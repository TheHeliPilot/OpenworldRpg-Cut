using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem current;
    public TooltipScript tooltip;

    public GameObject popoutTooptip;
    public GameObject areaNameTooptip;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //InvokeRepeating(nameof(RepeatHide), 0, 1);
    }

    private void RepeatHide()
    {
        HideTooltip();
    }
    
    public static void ShowTooltip(string text, string header = "")
    {
        current.tooltip.SetText(text, header);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void HideTooltip()
    {
        current.tooltip.gameObject.SetActive(false);
    }

    public static void ShowPopoutTooltip(string text)
    {
        current.popoutTooptip.GetComponent<Animation>().Play("PopoutTooltipAnimation");
        current.popoutTooptip.GetComponentInChildren<TMP_Text>().text = text;
    }

    public static void ShowAreaNameTooltip(string text)
    {
        current.areaNameTooptip.GetComponent<Animation>().Play("AreaNameAnimation");
        current.areaNameTooptip.GetComponentInChildren<TMP_Text>().text = text;
    }
}
