using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuff : MonoBehaviour
{
    // Damage
    public float damageMultiplier { get; set; }
    public float damageBonus { get; set; }
    // Health
    public float healthMultiplier { get; set; }
    public float healthBonus { get; set; }
    // Movement Speed
    public float movementMultiplier { get; set; }
    public float movementBonus { get; set; }
    // Melee Attack Speed
    public float atkSpdMultiplier { get; set; }
    public float atkSpdBonus { get; set; }
    // Ranged Fire Rate
    public float firerateMultiplier { get; set; }
    public float firerateBonus { get; set;}
}
