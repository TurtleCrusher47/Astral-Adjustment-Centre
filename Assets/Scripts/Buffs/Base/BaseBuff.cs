using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseBuff;

public class BaseBuff : MonoBehaviour
{
    public int buffId;

    // Buff Tiers
    public enum BuffTiers
    {
        TierI,
        TierII,
        TierIII,
        TierIV,
        TierV,
        // Can Add more if you want to
    }

    // Damage
    public float damageMultiplier { get; private set; } = 1.2f;
    public BuffTiers damageBuff { get; private set; } = BuffTiers.TierI;
    public string damageBuffString { get { return GetBuffTierstring(damageBuff); } }
    // Health
    public float healthMultiplier { get; private set; } = 1.5f;
    public BuffTiers healthBuff { get; private set; } = BuffTiers.TierI;
    public string healthBuffString { get { return GetBuffTierstring(healthBuff); } }
    // Movement Speed
    public float movementMultiplier { get; private set; } = 0.8f;
    public BuffTiers movementBuff { get; private set; } = BuffTiers.TierI;
    public string movementBuffString { get { return GetBuffTierstring(movementBuff); } }
    // Melee Attack Speed
    public float atkSpdMultiplier { get; private set; } = 0.5f;
    public BuffTiers atkSpdBuff { get; private set; } = BuffTiers.TierI;
    public string atkSpdBuffString { get { return GetBuffTierstring(atkSpdBuff); } }
    // Ranged Fire Rate
    public float firerateMultiplier { get; private set; } = 0.2f;
    public BuffTiers firerateBuff { get; private set; } = BuffTiers.TierI;
    public string firerateBuffString { get { return GetBuffTierstring(firerateBuff); } }

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
            case BuffTiers.TierI:
                return "I";
            case BuffTiers.TierII:
                return "II";
            case BuffTiers.TierIII:
                return "III";
            case BuffTiers.TierIV:
                return "IV";
            case BuffTiers.TierV:
                return "V";
            default:
                return "";
        }
    }

    // Function to get the multiplier for a specific tier
    public float GetMultiplierForTier(BuffTiers tier)
    {
        switch (tier)
        {
            case BuffTiers.TierI:
                return 1.2f;
            case BuffTiers.TierII:
                return 1.4f;
            case BuffTiers.TierIII:
                return 1.6f;
            case BuffTiers.TierIV:
                return 1.8f;
            case BuffTiers.TierV:
                return 2.0f;
            default:
                return 1.0f; // Default multiplier for unknown tier
        }
    }
}
