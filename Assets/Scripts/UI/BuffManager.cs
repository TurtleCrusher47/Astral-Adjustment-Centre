using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffManager : BaseBuff
{
    public GameObject roguePanel;

    [Header("Buff Panel")]
    public GameObject basePanel;

    public List<string> buffTitle = new List<string>();
    public List<string> buffDesc = new List<string>();

    // Just to see if its instantiating
    public List<GameObject> instantiatedPanels = new List<GameObject>(); // Keep track of instantiated panels

    // Start is called before the first frame update
    void Start()
    {
        //ShuffleBuffPanel();
        //InstantiateBuffPanels();
        //ShowRandomBuffPanels(2);
    }

    /*
    private void ShuffleBuffPanel()
    {
        int n = instantiatedPanels.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            GameObject temp = instantiatedPanels[k];
            instantiatedPanels[k] = instantiatedPanels[n];
            instantiatedPanels[n] = temp;
        }
    }*/

    private void InstantiateBuffPanels()
    {
        // Destroy the previous buff panels
        DestroyOldPanels();

        // Instantiate buff panels from the lists
        for (int i = 0; i < 2 && i < buffTitle.Count && i < buffDesc.Count; i++)
        {
            // Instantiate the base buff panel
            GameObject buffPanel = Instantiate(basePanel, roguePanel.transform);
            instantiatedPanels.Add(buffPanel); // Keep track of instantiated panel

            // Get the Title and Desc Text components from the instantiated panel
            TMP_Text titleText = buffPanel.transform.Find("TitleText").GetComponent<TMP_Text>();
            TMP_Text descText = buffPanel.transform.Find("DescText").GetComponent<TMP_Text>();

            // Set the text dynamically
            titleText.text = buffTitle[i];
            string buffDescription = $"Increases {buffTitle[i]} by {damageMultiplier}%";
            descText.text = buffDescription;

            // Add your logic here for showing the panels
            if (i == 0)
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

    private void DestroyOldPanels()
    {
        // Destroy the previous instantiated panels
        foreach (GameObject panel in instantiatedPanels)
        {
            Destroy(panel);
        }
        instantiatedPanels.Clear(); // Clear the list of instantiated panels
    }

    public void RerollButton()
    {
        // Destroy the previous buff panels before rerolling
        DestroyOldPanels();

        // Reroll logic
        //ShuffleBuffPanel();
        InstantiateBuffPanels();
        //ShowRandomBuffPanels(2);
    }
}
