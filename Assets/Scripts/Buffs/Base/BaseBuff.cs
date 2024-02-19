using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseBuff;

public class BaseBuff : MonoBehaviour
{
    // Buff Tiers
    public enum BuffTiers
    {
        Tier1,
        Tier2,
        Tier3,
        Tier4,
        Tier5,
        // Can Add more if you want to
    }

    // Damage
    public float damageMultiplier { get; private set; } = 1.2f;
    public BuffTiers damageBuff { get; private set; } = BuffTiers.Tier1;

    // Health
    public float healthMultiplier { get; private set; } = 1.5f;
    public BuffTiers healthBuff { get; private set; } = BuffTiers.Tier1;

    // Movement Speed
    public float movementMultiplier { get; private set; } = 0.8f;
    public BuffTiers movementBuff { get; private set; } = BuffTiers.Tier1;

    // Melee Attack Speed
    public float atkSpdMultiplier { get; private set; } = 0.5f;
    public BuffTiers atkSpdBuff { get; private set; } = BuffTiers.Tier1;

    // Ranged Fire Rate
    public float firerateMultiplier { get; private set; } = 0.2f;
    public BuffTiers firerateBuff { get; private set; } = BuffTiers.Tier1;

    // Function to update buff multiplier based on tier
    public void UpdateBuffMultiplier()
    {
        damageMultiplier = GetMultiplierForTier(damageBuff);
        healthMultiplier = GetMultiplierForTier(healthBuff);
        movementMultiplier = GetMultiplierForTier(movementBuff);
        atkSpdMultiplier = GetMultiplierForTier(atkSpdBuff);
        firerateMultiplier = GetMultiplierForTier(firerateBuff);
    }

    // Function to get the string representation of a buff tier
    public string GetBuffTierstring(BuffTiers tier)
    {
        switch (tier)
        {
            case BuffTiers.Tier1:
                return "I";
            case BuffTiers.Tier2:
                return "II";
            case BuffTiers.Tier3:
                return "III";
            case BuffTiers.Tier4:
                return "IV";
            case BuffTiers.Tier5:
                return "V";
            default:
                throw new ArgumentOutOfRangeException(nameof(tier), tier, "Invalid buff tier.");
        }
    }

    // Function to get the multiplier for a specific tier
    public float GetMultiplierForTier(BuffTiers tier)
    {
        switch (tier)
        {
            case BuffTiers.Tier1:
                return 1.0f;
            case BuffTiers.Tier2:
                return 1.2f;
            case BuffTiers.Tier3:
                return 1.4f;
            case BuffTiers.Tier4:
                return 1.6f;
            case BuffTiers.Tier5:
                return 1.8f;
            default:
                throw new ArgumentOutOfRangeException(nameof(tier), tier, "Invalid buff tier.");
        }
    }
}
