using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider healthSlider, easeHealthSlider;

    [SerializeField] 
    private TMP_Text healthText;

    [SerializeField]
    public PlayerData playerData;

    [Header("Ease Speed")]
    [SerializeField]
    private float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        playerData.currentHealth = playerData.currentMaxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != playerData.currentHealth)
        {
            healthSlider.value = playerData.currentHealth;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerData.currentHealth, lerpSpeed);
        }

        healthSlider.maxValue = playerData.currentMaxHealth;
        easeHealthSlider.maxValue = playerData.currentMaxHealth;
    }

    void FixedUpdate()
    {
        healthText.text = healthSlider.value.ToString() + " / " + playerData.currentMaxHealth;
    }

    public void Damage(float amount)
    {
        playerData.currentHealth -= amount;

        // Ensure health doesn't go below 0
        playerData.currentHealth = Mathf.Max(playerData.currentHealth, 0f);
    }

    public void Despawn(){return;}
}
