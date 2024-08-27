using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class ItemActionButtonScript : MonoBehaviour
{
    private AttackAction _myAction;
    private ActionSectionScript asc;

    public TMP_Text actionText;
    
    public void Setup(AttackAction action, ActionSectionScript a)
    {
        asc = a;
        _myAction = action;
        actionText.text = _myAction.actionName;
        GetComponent<TooltipHandler>().content = _myAction.actionDescription;
        GetComponent<TooltipHandler>().header = _myAction.staminaCost + " Stamina";
    }
    
    public void Click()
    {
        asc.DoAction(_myAction);
    }
}
