using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchScript : MonoBehaviour
{
    public GameObject watch;
    public RectTransform minuteHand;
    public RectTransform hourHand;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            watch.SetActive(true);
        if(Input.GetKeyUp(KeyCode.O))
            watch.SetActive(false);

        
    }

    private void FixedUpdate()
    {
        minuteHand.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(minuteHand.eulerAngles.z,  -6 * DayNightCycleScript.instance.currentDate.minutes, 3 * Time.fixedDeltaTime)));
        hourHand.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(hourHand.eulerAngles.z,  -30 * DayNightCycleScript.instance.currentDate.hours, 3 * Time.fixedDeltaTime)));
    }
}
