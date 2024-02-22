using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider, easeHealthSlider;

    private float maxHealth = 100f;
    private float health;
    //private float resetTime = 5f;
    //private float timer;

    [Header("Ease Speed")]
    [SerializeField]
    private float lerpSpeed = 0.005f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //timer = resetTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        /*
        // Check if health is 0 and initiate the reset timer
        if (health <= 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ResetHealth();
            }
        }*/
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        // Ensure health doesn't go below 0
        health = Mathf.Max(health, 0f);
    }

    /*
    private void ResetHealth()
    {
        health = maxHealth;
        timer = resetTime; // Reset the timer
    }*/
}
