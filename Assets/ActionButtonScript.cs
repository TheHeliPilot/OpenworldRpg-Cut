using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonScript : MonoBehaviour
{
    private bool _wasOff = false;
    
    private void LateUpdate()
    {
        if (!TurnBasedAlternativeManager.turnBasedAlternativeInstance.isInCombat){
            GetComponent<SlideInOutButton>().SetFunc(false);
            //GetComponent<SlideInOutButton>().Set(false);
        }
        
        switch (TurnBasedAlternativeManager.turnBasedAlternativeInstance.isInCombat)
        {
            case true when _wasOff:
                _wasOff = false;
                //GetComponent<SlideInOutButton>().buttonObject.GetComponent<Button>().interactable = true;
                break;
            case false:
                _wasOff = true;
                break;
        }

        //GetComponent<SlideInOutButton>().isFunctional =
        //    TurnBasedAlternativeManager.turnBasedAlternativeInstance.isInCombat;
    }
}
