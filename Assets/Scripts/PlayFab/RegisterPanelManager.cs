using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class RegisterPanelManager : MonoBehaviour
{
    [SerializeField] private MainSceneManager mainSceneManager;
    [SerializeField] TMP_Text statusText;
    [SerializeField] TMP_InputField if_displayname, if_username, if_email, if_password, if_confirmpassword;

    private List<TMP_InputField> fields;
    private int fieldIndexer;

    void Awake()
    {
        fields = new List<TMP_InputField>{if_displayname, if_username, if_email, if_password, if_confirmpassword};
        statusText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].isFocused)
                {
                    fieldIndexer = i + 1;
                }
            }

            if (fields.Count <= fieldIndexer)
            {
                fieldIndexer = 0;
            }

            fields[fieldIndexer].Select();
            fieldIndexer++;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnButtonRegUser();
        }
    }
    
    void OnError(PlayFabError e)
    {
        mainSceneManager.SetStatusText(e.ErrorMessage);
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(10);
        
        statusText.text = "";
    }
    
    public void OnButtonRegUser()
    {
        if (if_password.text == if_confirmpassword.text)
        {
            var regReq = new RegisterPlayFabUserRequest
            {
                Email = if_email.text,
                Password = if_password.text,
                Username = if_username.text,
                DisplayName = if_displayname.text
            };

            PlayFabClientAPI.RegisterPlayFabUser(regReq, OnRegisterSuccess, OnError);
        }
        else
        {
            mainSceneManager.SetStatusText("Passwords not matched.");
        }
        
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult r)
    {
        mainSceneManager.SetStatusText("Register Successful " + if_displayname.text + " !\nContinue on to Login Page.");

        if_displayname.text = string.Empty;
        if_username.text = string.Empty;
        if_email.text = string.Empty;
        if_password.text = string.Empty;
        if_confirmpassword.text = string.Empty;

        mainSceneManager.ShowLoginPage();
    }
}
