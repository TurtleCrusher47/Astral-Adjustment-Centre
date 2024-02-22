using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    [SerializeField]
    private GameObject roguePanel;

    [Header("Buff Data")]
    public List<ScriptableBuff> buffs;

    public PlayerData playerData;

    public TMP_Text rerollText;

    [Header("Buff Panel")]
    public GameObject basePanel;

    // Just to see if its instantiating
    private List<GameObject> instantiatedPanels = new List<GameObject>(); // Keep track of instantiated panels

    // Start is called before the first frame update
    void Awake()
    {
        // Move this to Awake Function or Call them. When player interacts with Chest/Wateve idk
        //ShuffleBuffPanel();
        //InstantiateBuffPanels();
    }

    void Start()
    {
        rerollText.text = "2";
    }

    private void ShuffleBuffList()
    {
        int n = buffs.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            ScriptableBuff temp = buffs[k];
            buffs[k] = buffs[n];
            buffs[n] = temp;
        }
    }

    private void InstantiateBuffPanels()
    {
        // Shuffle the buff order to randomize panel positions
        ShuffleBuffList();

        // Instantiate buff panels from the lists
        for (int i = 0; i < 2 && i < buffs.Count; i++)
        {
            // Instantiate the base buff panel
            GameObject buffPanel = Instantiate(basePanel, roguePanel.transform);
            instantiatedPanels.Add(buffPanel);
            //buffIndices.Add(i); // Store the buff index for this panel

            // Get the Title and Desc Text components
            TMP_Text titleText = buffPanel.transform.Find("TitleText").GetComponent<TMP_Text>();
            TMP_Text descText = buffPanel.transform.Find("DescText").GetComponent<TMP_Text>();

            // Set text dynamically using generic buff properties
            titleText.text = buffs[i].buffName + " " + buffs[i].buffTiers[0]; // Show Level 1 initially

            descText.text = "Increases " + buffs[i].buffName + " by " + buffs[i].buffBonus[0] + " %";

            // Add click listener to the button
            Button button = buffPanel.GetComponent<Button>();
            if (button != null)
            {
                int index = i; // Capture loop index for OnPanelClick
                button.onClick.AddListener(() => OnPanelClick(index));
            }

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
        foreach (GameObject panel in instantiatedPanels)
        {
            Destroy(panel);
        }
        instantiatedPanels.Clear();
    }

    public void RerollButton()
    {
        DestroyOldPanels();

        

        InstantiateBuffPanels();
    }

    public void ClearButton()
    {
        DestroyOldPanels();
    }

    /*
    public void ResetBuffs()
    {
        ScriptableBuff.ResetBuffTier();
        buffs.ResetAttack();
        buffs.ResetAtkSpd();
        buffs.ResetFireRate();
        buffs.ResetHealth();
        buffs.ResetSpeed();
    }*/

    private void OnPanelClick(int index)
    {
        // Ensure the index is valid
        if (index >= 0 && index < buffs.Count)
        {
            // Get the selected buff
            ScriptableBuff selectedBuff = buffs[index];

            // Update PlayerData based on the selected buff
            switch (selectedBuff.buffName)
            {
                case "Speed":
                    playerData.speedLevel += 1;
                    playerData.walkSpeed *= 1 + selectedBuff.buffBonus[0];
                    break;

                case "Health":
                    playerData.healthLevel += 1;
                    // Buff HP
                    break;

                case "Attack":
                    playerData.attackLevel += 1;
                    // Buff Player
                    break;

                case "Atk Spd":
                    playerData.atkSpeedLevel += 1;
                    // Buff Atk Spd
                    break;

                case "Fire Rate":
                    playerData.fireRateLevel += 1;
                    // Buff Fire Rate
                    break;

                // Add more cases if you have additional buff types

                default:
                    Debug.LogWarning("Unknown buff type: " + selectedBuff.buffName);
                    break;
            }

            // Get the player's level for the selected buff
            int playerLevel = 0;

            switch (selectedBuff.buffName)
            {
                case "Speed":
                    playerLevel = playerData.speedLevel;
                    break;

                case "Health":
                    playerLevel = playerData.healthLevel;
                    break;

                case "Attack":
                    playerLevel = playerData.attackLevel;
                    break;

                case "Atk Spd":
                    playerLevel = playerData.atkSpeedLevel;
                    break;

                case "Fire Rate":
                    playerLevel = playerData.fireRateLevel;
                    break;
                default:
                    Debug.LogWarning("Unknown buff type: " + selectedBuff.buffName);
                    break;
            }

            // Clamp playerLevel between the array
            int arrayIndex = Mathf.Clamp(playerLevel, 0, selectedBuff.buffTiers.Length - 1);

            // Use playerLevel as an index for buffTiers and buffBonus arrays
            selectedBuff.buffTiers[0] = selectedBuff.buffTiers[arrayIndex];
            selectedBuff.buffBonus[0] = selectedBuff.buffBonus[arrayIndex];

            Debug.Log("Level: " + playerLevel + "\n" + "Buff Selected: " + selectedBuff + "\n" + "Buff Bonus: " + selectedBuff.buffBonus);

            // Reroll panels with the updated levels, tiers, and bonus
            DestroyOldPanels();
            InstantiateBuffPanels();
        }
    }
}
