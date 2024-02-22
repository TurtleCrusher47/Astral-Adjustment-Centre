using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player/Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
	[SerializeField] public float moveSpeed = 25.0f;
	[SerializeField] public float airMultiplier = 0.5f;

	[Header("Sprinting")]
	[SerializeField] public float walkSpeed = 35f;
	[SerializeField] public float sprintSpeed = 55f;
	[SerializeField] public float acceleration = 15f;

	[Header("Jumping")]
	public float jumpForce = 6f;

	[Header("Keybinds")]
	[SerializeField] public KeyCode jumpKey = KeyCode.Space;
	[SerializeField] public KeyCode sprintKey = KeyCode.LeftShift;
	[SerializeField] public KeyCode crouchKey = KeyCode.LeftControl;
	[SerializeField] public KeyCode proneKey = KeyCode.C;

	[Header("Crouch and Prone")]
	[SerializeField] public float crouchScale = 0.5f;
	[SerializeField] public float crouchSpeed = 25f;
	[SerializeField] public float proneScale = 0.2f;
	[SerializeField] public float proneSpeed = 15f;
	public float standScale = 1f;
	
	[Header("Ground Detection")]
	[SerializeField] public LayerMask groundMask;
	public float groundDistance = 0.1f;

	[Header("Drag")]
	public float groundDrag = 6f;
	public float airDrag = 2f;

    [Header("Health")]
    public float currentHealth = 100;
    public float initialMaxHealth = 100;
	public float currentMaxHealth = 100;

	[Header("Player Settings")]
	public float mainVolume;
	public float sfxVolume;

	[Header("Buffs")]
	public int healthLevel;
	public int attackLevel;
	public int speedLevel;
	public int atkSpeedLevel;
	public int fireRateLevel;

    public void ResetValues()
    {
        currentHealth = initialMaxHealth;
		currentMaxHealth = initialMaxHealth;

		UpdateHealth();
        ResetBuffs();
    }

	public void UpdateHealth()
	{
        ScriptableBuff hpBuff = BuffManager.Instance.buffs[3];
        float hpBuffMultiplier;
        if (hpBuff.currBuffTier > 0)
        {
            hpBuffMultiplier = hpBuff.buffBonus[hpBuff.currBuffTier - 1];
        }
		else
        {
            hpBuffMultiplier = 1;
        }

        float temp = currentMaxHealth;
        currentMaxHealth = initialMaxHealth * hpBuffMultiplier;
		currentHealth += currentMaxHealth - temp;
	}

	public void ResetBuffs()
	{
        // Reset buff
		healthLevel = 0;
		attackLevel = 0;
		speedLevel = 0;
		atkSpeedLevel = 0;
		fireRateLevel = 0;
    }
}
