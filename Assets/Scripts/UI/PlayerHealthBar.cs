using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour, IDamageable
{
    [SerializeField] private Slider healthSlider, easeHealthSlider;

    public PlayerData playerData;

    //private float maxHealth = 100f;
    //private float health;

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

        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(10);
        }*/

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, playerData.currentHealth, lerpSpeed);
        }
    }

    public void Damage(float amount)
    {
        playerData.currentHealth -= amount;

        // Ensure health doesn't go below 0
        playerData.currentHealth = Mathf.Max(playerData.currentHealth, 0f);
    }

    public void Despawn(){return;}
}
