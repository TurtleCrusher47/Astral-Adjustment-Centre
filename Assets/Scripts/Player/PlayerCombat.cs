using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageable
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] private TMP_Text healthText;

    void Start()
    {
        playerData.ResetValues();
        healthText.text = playerData.currentHealth.ToString() + " HP";
    }

    public void Damage(float damage)
    {
        playerData.currentHealth -= damage;
        if (playerData.currentHealth < 0)
        playerData.currentHealth = 0;
        
        healthText.text = playerData.currentHealth.ToString() + " HP";
    }

    public void Despawn()
    {
        throw new System.NotImplementedException();
    }
}
