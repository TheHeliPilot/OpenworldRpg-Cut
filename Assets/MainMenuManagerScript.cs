using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using MainManagers;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManagerScript : MonoBehaviour
{
    public static bool[] cgUnlock;

    public TMP_Text loadingPercentageText;
    public GameObject[] CGs;
    public Transform CGSpawn;
    public GameObject menu;
    public GameObject map;
    public Transform galleryScreen;
    
    private bool _spawnedCg;
    private GameObject _spawnedCgObject;

    private void Awake()
    {
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/logs");
        LogFile.path = Application.persistentDataPath + "/logs/log_" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture).Replace('/', '-').Replace(' ', '_').Replace(':', '-') + ".txt";
        
        SaveManager.LoadCGs(Application.persistentDataPath + "/cgdata.zues", galleryScreen.childCount);
        SaveManager.SaveCGs(Application.persistentDataPath + "/cgdata.zues", cgUnlock);
    }

    private void Start()
    {
        SteamIntegration.Init();
        SteamIntegration.UnlockAchievement("START_GAME");
        
        MainAudioManager.instance.playAmbience = false;

        //CG LOCKING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        for (int i = 0; i < cgUnlock.Length; i++)
        {
            //galleryScreen.GetChild(i).GetComponent<Button>().interactable = cgUnlock[i];
        }
    }

    public void LoadLevel(int index)
    {
        StartCoroutine(LoadAsynchronously(index));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCg(int cg)
    {
        CGManager.IsInCG = true;
        _spawnedCgObject = Instantiate(CGs[cg], CGSpawn);
        _spawnedCgObject.transform.localScale = new Vector3(0.02541483f, 0.02541483f, 0.02541483f);
    }

    private void Update()
    {
        if (CGManager.IsInCG && !_spawnedCg) _spawnedCg = true;

        if (!CGManager.IsInCG && _spawnedCg)
        {
            Destroy(_spawnedCgObject);
            _spawnedCg = false;
        }

        if (CGManager.IsInCG && _spawnedCg)
        {
            CGSpawn.gameObject.SetActive(true);
            map.SetActive(false);
            menu.SetActive(false);
        }

        if (!CGManager.IsInCG && !_spawnedCg)
        {
            CGSpawn.gameObject.SetActive(false);
            map.SetActive(true);
            menu.SetActive(true);
        }
    }

    private IEnumerator LoadAsynchronously(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingPercentageText.text = progress * 100 + "%";

            yield return null;
        }
    }
}
