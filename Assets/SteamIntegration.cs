using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;

public static class SteamIntegration
{
    public static void Init()
    {
        Debug.Log($"Steam innit: {Steamworks.SteamClient.AppId}");
        try
        {
            Steamworks.SteamClient.Init(2185410);
        }
        catch(Exception e)
        {
            Debug.LogError($"Steam innit error!");
            Debug.LogException(e);
        }
        Debug.Log($"Steam initialised!");
    }

    public static void UnlockAchievement(string id)
    {
        Achievement a = new(id);
        a.Trigger();
    }
}
