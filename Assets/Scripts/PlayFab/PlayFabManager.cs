using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.AuthenticationModels;
using PlayFab.ClientModels;
using EntityKey = PlayFab.GroupsModels.EntityKey;
using UnityEngine;
using PlayFab.GroupsModels;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance = null;
    public static string currPlayFabID;
    public static string currTitleID;
    public static string currPlayFabDN;
    public static EntityKey currGuildID;
    public static string currGuildName;
    public static string currGuildRole;

    public static string nextSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(base.gameObject); 
        }
        else
        {
            Destroy(base.gameObject);
        }
    }
    
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScene");

        nextSceneName = sceneName;
    }
}
