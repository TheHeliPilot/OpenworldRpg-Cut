using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider effectsSlider;
    public AudioMixer mixer;
    public bool toggleOnEscape;

    private void Update()
    {
        if (!toggleOnEscape) return;
        
        if(Input.GetKeyDown(KeyCode.Escape))
            GetComponent<SlideInOutButton>().Toggle();
    }

    public void SetVolume()
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
        mixer.SetFloat("EffectsVolume", Mathf.Log10(effectsSlider.value) * 20);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
