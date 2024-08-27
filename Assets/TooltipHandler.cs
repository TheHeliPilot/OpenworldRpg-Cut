using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    public string content;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.ShowTooltip(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.HideTooltip();
    }
}
