using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject phonePanel;

    private bool isPhonePanelOpen = false;

    private void Start()
    {
        ResetPanels();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isPhonePanelOpen)
            {
                ClosePhone();
            }
            else
            {
                OpenPhone();
            }
        }
    }

    public void OpenPhone()
    {
        OpenPhoneAnimation();
    }

    public void ClosePhone()
    {
        ClosePhoneAnimation();
    }

    public void OpenOption()
    {
        OpenOptionAnimation();
    }

    public void CloseOption()
    {
        CloseOptionAnimation();
    }

    public void ExitToMainMenu()
    {
        LoadMainMenuScene();
    }
    
    private void OpenPhoneAnimation()
    {
        LeanTween.moveLocal(phonePanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutQuart);
        LeanTween.scale(phonePanel, new Vector3(1f, 1f, 1f), 1f).setDelay(0.2f).setEase(LeanTweenType.easeInOutSine);

        // Show the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Time.timeScale = 0f;

        isPhonePanelOpen = true;
    }

    private void ClosePhoneAnimation()
    {
        Time.timeScale = 1f;
        isPhonePanelOpen = false;

        LeanTween.scale(phonePanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveLocal(phonePanel, new Vector3(791f, 464f, 0f), .5f).setDelay(0.2f).setEase(LeanTweenType.easeInOutQuart);

        // Show the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Time.timeScale = 1f;

        isPhonePanelOpen = false;
    }

    private void OpenOptionAnimation()
    {
        //LeanTween.scale(optionPanel, new Vector3(1f, 1f, 1f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(phonePanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
    }

    private void CloseOptionAnimation()
    {
        //LeanTween.scale(optionPanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(phonePanel, new Vector3(1f, 1f, 1f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
    }

    private void ResetPanels()
    {
        // Reset PhonePanel
        phonePanel.transform.localPosition = new Vector3(791f, 464f, 0f);
        phonePanel.transform.localScale = new Vector3(0f, 0f, 0f);

        // Reset Option Panel
        //optionPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
        //optionPanel.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
