using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject phonePanel, optionPanel;

    [Header("GameObjects")]
    public GameObject optionButton, exitButton;

    [Header("Buff Panels")]
    public List<GameObject> buffPanels = new List<GameObject>();

    private void Start()
    {
        // Reset PhonePanel
        phonePanel.transform.localPosition = new Vector3(791f, 464f, 0f);
        phonePanel.transform.localScale = new Vector3(0f, 0f, 0f);

        // Reset Option Panel
        optionPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
        optionPanel.transform.localScale = new Vector3(0f, 0f, 0f);

        StartBuff();

        ShuffleBuffPanel();

        Debug.Log("GameObjects in List: " + buffPanels.Count);

        foreach( GameObject buffPanel in buffPanels)
        {
            Debug.Log("GameObject Name: " + buffPanel.name);
        }
    }

    private void StartBuff()
    {
        // Reset Buff Panel
        //buffPanels.transform.localPosition = new Vector3(-344.25f, 61f, 0f);
        //buffPanel.transform.localScale = new Vector3(1f, 1f, 0f);
        // buffPanel.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        //buffPanel2.transform.localPosition = new Vector3(388f, 126f, 0f);
        //buffPanel2.transform.localScale = new Vector3(1f, 1f, 0f);
        //buffPanel2.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        // Test Buff 
        //LeanTween.moveLocal(buffPanel, new Vector3(-344.25f, 0f, 0f), .6f).setDelay(0.3f).setEase(LeanTweenType.easeInOutQuart);
        // LeanTween.rotate(buffPanel, new Vector3(0f, 0f, 0f), .9f).setDelay(1.2f).setEase(LeanTweenType.easeInOutExpo);

        //LeanTween.moveLocal(buffPanel2, new Vector3(384f, 0f, 0f), .6f).setDelay(0.5f).setEase(LeanTweenType.easeInOutQuart);
        //LeanTween.rotate(buffPanel2, new Vector3(0f, 0f, 0f), .9f).setDelay(1.4f).setEase(LeanTweenType.easeInOutExpo);
    }

    private void ShuffleBuffPanel()
    {
        int n = buffPanels.Count;
        while ( n > 1)
        {
            n--;
            int k  = Random.Range(0, n + 1);
            GameObject temp = buffPanels[k];
            buffPanels[k] = buffPanels[n];
            buffPanels[n] = temp;
        }
    }

    public void RerollButton()
    {
        ShuffleBuffPanel();

        // Reset Buff Panel
        //buffPanel.transform.localPosition = new Vector3(-344.25f, 61f, 0f);
        //buffPanel.transform.localScale = new Vector3(1f, 1f, 0f);
        //buffPanel.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        //buffPanel2.transform.localPosition = new Vector3(388f, 126f, 0f);
        //buffPanel2.transform.localScale = new Vector3(1f, 1f, 0f);
        //buffPanel2.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        // Test Buff Animations 
        //LeanTween.moveLocal(buffPanel, new Vector3(-344.25f, 0f, 0f), .6f).setDelay(0.3f).setEase(LeanTweenType.easeInOutQuart);
        //LeanTween.rotate(buffPanel, new Vector3(0f, 0f, 0f), .9f).setDelay(1.2f).setEase(LeanTweenType.easeInOutExpo);

        //LeanTween.moveLocal(buffPanel2, new Vector3(384f, 0f, 0f), .6f).setDelay(0.5f).setEase(LeanTweenType.easeInOutQuart);
        //LeanTween.rotate(buffPanel2, new Vector3(0f, 0f, 0f), .9f).setDelay(1.4f).setEase(LeanTweenType.easeInOutExpo);
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
}
