using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject phonePanel;
    public GameObject optionPanel;
    public GameObject roguePanel;

    private void Start()
    {
        // Reset PhonePanel
        phonePanel.transform.localPosition = new Vector3(791f, 464f, 0f);
        phonePanel.transform.localScale = new Vector3(0f, 0f, 0f);

        // Reset Option Panel
        optionPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
        optionPanel.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            OpenPhone();
        }
        else if (Input.GetButtonDown("Cancel"))
        {
            ClosePhone();
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
    }

    private void ClosePhoneAnimation()
    {
        LeanTween.scale(phonePanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.moveLocal(phonePanel, new Vector3(791f, 464f, 0f), .5f).setDelay(0.2f).setEase(LeanTweenType.easeInOutQuart);
    }

    private void OpenOptionAnimation()
    {
        LeanTween.scale(optionPanel, new Vector3(1f, 1f, 1f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(phonePanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
    }

    private void CloseOptionAnimation()
    {
        LeanTween.scale(optionPanel, new Vector3(0f, 0f, 0f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.scale(phonePanel, new Vector3(1f, 1f, 1f), .5f).setDelay(0.1f).setEase(LeanTweenType.easeInOutSine);
    }

    private void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
