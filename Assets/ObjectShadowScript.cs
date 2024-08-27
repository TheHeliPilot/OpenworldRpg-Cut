using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectShadowScript : MonoBehaviour
{
    public bool shouldCheck;
    public Vector3 morningPos;
    public Vector3 morningScale;
    public Vector3 noonPos;
    public Vector3 noonScale;
    public Vector3 eveningPos;
    public Vector3 eveningScale;
    public Transform shadow;
    
    private CustomDate _date;

    private float _currPos;
    private float _wantedPos;

    private void Start()
    {
        StartCoroutine(CheckTime());
    }

    private void FixedUpdate()
    {
        //_currPos = Mathf.Lerp(_currPos, _wantedPos, Time.fixedDeltaTime * .1f);
    }

    private IEnumerator CheckTime()
    {
        while (shouldCheck)
        {
            //yield return new WaitForSeconds(Random.Range(30f, 60f));
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));

            _date = DayNightCycleScript.instance.currentDate;

            _currPos = _date.hours / 12f - 1f;

            shadow.localPosition = Vector3.Lerp(noonPos, _currPos < 0 ? morningPos : eveningPos, Mathf.Abs(_currPos));
            shadow.localScale = Vector3.Lerp(noonScale, _currPos < 0 ? morningScale : eveningScale, Mathf.Abs(_currPos));
            shadow.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0,Mathf.Clamp01(1 - Mathf.Abs(_currPos) - .2f));
        }
    }
}
