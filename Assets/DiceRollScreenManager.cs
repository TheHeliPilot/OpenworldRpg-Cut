using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DiceRollScreenManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text rollText;
    [SerializeField]
    private TMP_Text statusText;
    [SerializeField]
    private TMP_Text typeText;

    private static DiceRollScreenManager _instance;

    private void Start()
    {
        _instance = this;
        active = false;
    }
    
    public static bool active = false;

    private void Update()
    {
        transform.GetChild(0).gameObject.SetActive(active);
    }

    public static void SetRoll(int roll, int die = 6)
    {
        _instance.rollText.text = roll.ToString();
    }

    public static void Finish(bool win, string _override = "")
    {
        if (_override != "")
            _instance.statusText.text = _override;
        else
            _instance.statusText.text = win ? "Success" : "Fail";
    }

    public static void SetType(Skills skill)
    {
        Debug.Log($"Skill set: {skill}");
        
        if (skill == Skills.none)
            _instance.typeText.text = "Skill Roll";
        else
            _instance.typeText.text = "Roll: " + skill;
    }
}
