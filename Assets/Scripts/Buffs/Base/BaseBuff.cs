using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : MonoBehaviour
{
    public int id;

    // Buff Tiers
    public enum buffTiers
    {
        TierI,
        TierII,
        TierIII,
        TierIV,
        TierV,
        // Can Add more if you want to
    }

    // Damage
    public float damageMultiplier { get; set; } = 1.2f;
    public buffTiers damageBuff {  get; set; } = buffTiers.TierI; 
    // Health
    public float healthMultiplier { get; set; } = 1.5f;
    public buffTiers healthBuff { get; set; } = buffTiers.TierI;
    // Movement Speed
    public float movementMultiplier { get; set; } = 0.8f;
    public buffTiers movementBuff { get; set; } = buffTiers.TierI;
    // Melee Attack Speed
    public float atkSpdMultiplier { get; set; } = 0.5f;
    public buffTiers atkSpdBuff { get; set; } = buffTiers.TierI;
    // Ranged Fire Rate
    public float firerateMultiplier { get; set; } = 0.2f;
    public buffTiers firerateBuff { get; set; } = buffTiers.TierI;
}
