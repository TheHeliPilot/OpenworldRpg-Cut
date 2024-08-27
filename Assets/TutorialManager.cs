using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainManagers;
using TMPro;
using UnityEngine;

[Serializable]
public class Tutorial
{
    public Vector2[] circlePositions;
    public Vector2[] textPositions;
    public string[] texts;
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public bool isInTutorial;
    
    public GameObject tutorial;
    public RectTransform tutorialCircle;
    public RectTransform text;
    public AudioCue paperSlide;
    
    private List<Vector2> _circlePos = new();
    private List<Vector2> _textPos = new();
    private List<string> _texts = new();
    private Vector2 _circleCurr;
    private Vector2 _textCurr;
    private void Awake()
    {
        instance = this;
    }

    public void StartTutorial(Tutorial tutorial)
    {
        isInTutorial = true;
        this.tutorial.SetActive(true);
        _circlePos = tutorial.circlePositions.ToList();
        _textPos = tutorial.textPositions.ToList();
        _texts = tutorial.texts.ToList();
        Next();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isInTutorial)
            Next();
    }

    public void Next()
    {
        MainAudioManager.instance.SpawnAudio(paperSlide.GetSound(), Camera.main.transform.position, paperSlide.volume, 150, false, false);
        
        if (_circlePos.Count == 0)
        {
            isInTutorial = false;
            tutorial.SetActive(false);
            return;
        }

        _circleCurr = _circlePos[0];
        _circlePos.RemoveAt(0);
        _textCurr = _textPos[0];
        _textPos.RemoveAt(0);
        text.gameObject.GetComponent<TMP_Text>().text = _texts[0];
        _texts.RemoveAt(0);
    }

    private void FixedUpdate()
    {
        float speed = 4f;
        tutorialCircle.anchoredPosition =
            new Vector2(Mathf.Lerp(tutorialCircle.anchoredPosition.x, _circleCurr.x, speed * Time.fixedDeltaTime),
                Mathf.Lerp(tutorialCircle.anchoredPosition.y, _circleCurr.y, speed * Time.fixedDeltaTime));
        text.anchoredPosition =
            new Vector2(Mathf.Lerp(text.anchoredPosition.x, _textCurr.x, speed * Time.fixedDeltaTime),
                Mathf.Lerp(text.anchoredPosition.y, _textCurr.y, speed * Time.fixedDeltaTime));
    }
}
