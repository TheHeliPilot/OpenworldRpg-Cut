using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[Serializable]
public class CustomDate
{
    public int year;
    public int month;
    public int day;
    public int hours;
    public int minutes;
    public int seconds;
}

public class DayNightCycleScript : MonoBehaviour
{
    public static DayNightCycleScript instance;
    
    public DateTime date;
    public CustomDate currentDate = new();
    public Light2D sun;
    public Gradient sunColor;
    [Range(0f, 1f)] public float fogStrength;
    [Range(0f, 1f)] public float rainStrength;
    [Range(0f, 1f)] public float snowStrength;

    public GameObject[] fogObjects;
    public ParticleSystem rainSystem;
    public ParticleSystem snowSystem;

    public int minAdd;
    
    private static readonly int Strength = Shader.PropertyToID("Strength");
    private Color _sunWantedColor = Color.white;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        date = date.AddYears(currentDate.year);
        date = date.AddMonths(currentDate.month);
        date = date.AddDays(currentDate.day);
        date = date.AddHours(currentDate.hours);
        date = date.AddMinutes(currentDate.minutes);
        date = date.AddSeconds(currentDate.seconds);
        
        InvokeRepeating(nameof(UpdateTime), 0, 1);
    }

    [Obsolete("Obsolete")]
    private void Update()
    {
        float curr;
        if (date.Hour > 20 && date.Hour < 5)
            curr = 0;
        else
            curr = date.Hour switch
            {
                < 11 => (date.Hour * 60f + date.Minute - 5f * 60f) / (6f * 60f),
                >= 11 and <= 13 => 1,
                > 13 => 1 - (date.Hour * 60f + date.Minute - 13f * 60f) / (7f * 60f)
            };

        sun.intensity = curr;

        foreach (GameObject fogObject in fogObjects)
        {
            fogObject.GetComponent<SpriteRenderer>().material.SetFloat("_FogStrength", fogStrength * 2);
        }

        rainSystem.emissionRate = rainStrength * 800;
        snowSystem.emissionRate = snowStrength * 800;

        _sunWantedColor = sunColor.Evaluate(date.Hour / 24f);
        sun.color = Color.Lerp(sun.color, _sunWantedColor, Time.deltaTime * .1f);
    }

    private void UpdateTime()
    {
        date = date.Add(new TimeSpan(0, minAdd, 10));
        currentDate.year = date.Year;
        currentDate.month = date.Month;
        currentDate.day = date.Day;
        currentDate.hours = date.Hour;
        currentDate.minutes = date.Minute;
        currentDate.seconds = date.Second;
    }
}
