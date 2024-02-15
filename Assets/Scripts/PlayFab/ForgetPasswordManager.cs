using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class ForgetPasswordManager : MonoBehaviour
{
    [SerializeField] TMP_Text txt_info;
    [SerializeField] TMP_InputField if_email;
    [SerializeField] Button btn_send;

    private string initialInfoText;

    void Awake()
    {
        initialInfoText = "Enter the email address you registered for your game account, and we will send you an email message with password reset information.";
        txt_info.text = initialInfoText;
    }
    
    public void UpdateMessage(string msg)
    {
        Debug.Log(msg);
        txt_info.text = msg;
    }
    
    void OnError(PlayFabError e)
    {
        UpdateMessage("Error Occurred.\nPlease re-enter your email address.");
    }

    public void OnButtonResetPassword()
    {
        var emailReq = new SendAccountRecoveryEmailRequest
        {
            TitleId = PlayFabSettings.TitleId,
            Email = if_email.text
        };

        PlayFabClientAPI.SendAccountRecoveryEmail(emailReq, OnEmailSentSuccess, OnError);
    }

    void OnEmailSentSuccess(SendAccountRecoveryEmailResult r)
    {
        txt_info.text = "Recovery email has been sent ! ";
        btn_send.interactable = false;

        try
        {
            StartCoroutine(ResetText());
        }
        catch
        {
            Debug.Log("Left Forget Password Panel.");
        }
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(10);
        
        txt_info.text = initialInfoText;
    }
}
