using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject phonePanel, optionPanel, roguePanel;

    [Header("GameObjects")]
    public GameObject optionButton, exitButton;

    [Header("Buff Panels")]
    public List<GameObject> buffPanels = new List<GameObject>();

    private List<GameObject> instantiatedPanels = new List<GameObject>(); // Keep track of instantiated panels

    private void Start()
    {
        // Reset PhonePanel
        phonePanel.transform.localPosition = new Vector3(791f, 464f, 0f);
        phonePanel.transform.localScale = new Vector3(0f, 0f, 0f);

        // Reset Option Panel
        optionPanel.transform.localPosition = new Vector3(0f, 0f, 0f);
        optionPanel.transform.localScale = new Vector3(0f, 0f, 0f);

        //ShuffleBuffPanel();
        //InstantiateBuffPanels();
        //ShowRandomBuffPanels(2);

        //Debug.Log("GameObjects in List: " + buffPanels.Count);

        /*
        foreach( GameObject buffPanel in buffPanels)
        {
            Debug.Log("GameObject Name: " + buffPanel.name);
        }*/
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

    private void InstantiateBuffPanels()
    {
        // Destroy the previous buff panels
        DestroyOldPanels();

        // Instantiate buff panels from the list
        for (int i = 0; i < 2 && i < buffPanels.Count; i++)
        {
            GameObject buffPanel = Instantiate(buffPanels[i], roguePanel.transform);
            instantiatedPanels.Add(buffPanel); // Keep track of instantiated panel
        }
    }

    private void DestroyOldPanels()
    {
        // Destroy the previous instantiated panels
        foreach (GameObject panel in instantiatedPanels)
        {
            Destroy(panel);
        }
        instantiatedPanels.Clear(); // Clear the list of instantiated panels
    }

    private void ShowRandomBuffPanels(int numberOfPanelsToShow)
    {
        // Ensure there are enough buff panels in the list
        if (numberOfPanelsToShow > buffPanels.Count)
        {
            Debug.LogWarning("Not enough buff panels in the list.");
            return;
        }

        // Show the specified number of random buff panels
        for (int i = 0; i < numberOfPanelsToShow; i++)
        {
            GameObject buffPanel = instantiatedPanels[i]; // Use the instantiated panels
            // Add your logic here for showing the panels
            if(i == 0)
            {
                // Reset Buff Panel
                buffPanel.transform.localPosition = new Vector3(-344.25f, 61f, 0f);
                buffPanel.transform.localScale = new Vector3(1f, 1f, 0f);
                buffPanel.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                // Buff Animation
                LeanTween.moveLocal(buffPanel, new Vector3(-344.25f, 0f, 0f), .6f).setDelay(0.1f).setEase(LeanTweenType.easeInOutQuart);
                LeanTween.rotate(buffPanel, new Vector3(0f, 0f, 0f), .9f).setDelay(1f).setEase(LeanTweenType.easeInOutExpo);
            }
            else if (i == 1)
            {
                buffPanel.transform.localPosition = new Vector3(388f, 126f, 0f);
                buffPanel.transform.localScale = new Vector3(1f, 1f, 0f);
                buffPanel.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                // Buff Animation
                LeanTween.moveLocal(buffPanel, new Vector3(384f, 0f, 0f), .6f).setDelay(0.3f).setEase(LeanTweenType.easeInOutQuart);
                LeanTween.rotate(buffPanel, new Vector3(0f, 0f, 0f), .9f).setDelay(1.2f).setEase(LeanTweenType.easeInOutExpo);
            }
        }
    }

    public void RerollButton()
    {
        // Destroy the previous buff panels before rerolling
        DestroyOldPanels();

        // Reroll logic
        ShuffleBuffPanel();
        InstantiateBuffPanels();
        ShowRandomBuffPanels(2);
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
