using System.Collections;
using System.Collections.Generic;
using MainManagers;
using UnityEngine;
using UnityEngine.UI;

public class CustomStoryScriptManager : MonoBehaviour
{
    public void DoSomething(int i)
    {
        switch (i)
        {
            case 0:
                
                break;
            
            
            default:
                Debug.LogWarning($"Nothing with index {i} exists in StoryManager!");
                break;
        }
    }
}
