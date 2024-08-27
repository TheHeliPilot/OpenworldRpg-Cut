using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;

public class ChoiceButtonScript : MonoBehaviour
{
    public bool isInteractChoice;
    public TriggerHandlerScript ths;
    public int choice;
    public Skills skill;
    public void Pressed()
    {
        if (isInteractChoice)
        {
            ths.Selected(choice);
            return;
        }

        MainDialogueManager.Instance.StartChoice(choice, skill);
    }
}
