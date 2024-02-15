using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordInputField : MonoBehaviour
{
    private TMP_InputField passwordInput;

    void Start()
    {
        passwordInput = GetComponent<TMP_InputField>();
        passwordInput.contentType = TMP_InputField.ContentType.Password;
    }

    public void ToggleInputType()
    {
        if (passwordInput.contentType == TMP_InputField.ContentType.Password)
        {
            passwordInput.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;
        }

        passwordInput.ForceLabelUpdate();
    }
}