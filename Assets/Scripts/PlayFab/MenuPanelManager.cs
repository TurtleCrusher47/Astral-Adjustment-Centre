using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;

public class MenuPanelManager : MonoBehaviour
{
    [SerializeField] TMP_Text statusText;

    public void UpdateMessage(string msg)
    {
        Debug.Log(msg);
        statusText.text = msg;
    }
    
    void OnError(PlayFabError e)
    {
        UpdateMessage("Error : " + e);
    }

    
}
