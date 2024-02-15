using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : MonoBehaviour
{

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
    public float damageMultiplier { get; set; }
    public buffTiers DamageBuff {  get; set; } = buffTiers.TierI; 
    // Health
    public float healthMultiplier { get; set; }
    public buffTiers healthBuff { get; set; } = buffTiers.TierI;
    // Movement Speed
    public float movementMultiplier { get; set; }
    public buffTiers movementBuff { get; set; } = buffTiers.TierI;
    // Melee Attack Speed
    public float atkSpdMultiplier { get; set; }
    public buffTiers atkSpdBuff { get; set; } = buffTiers.TierI;
    // Ranged Fire Rate
    public float firerateMultiplier { get; set; }
    public buffTiers firerateBuff { get; set; } = buffTiers.TierI;
}
