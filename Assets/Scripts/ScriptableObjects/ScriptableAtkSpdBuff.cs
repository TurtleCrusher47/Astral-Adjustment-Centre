using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AtkSpdBuff")]
public class ScriptableAtkSpdBuff : ScriptableBuff
{
    // Constructor to set default values
    public ScriptableAtkSpdBuff()
    {
        // Default Assign the name
        buffName = "Atk Spd";
        
        buffTiers = new string[] { "I", "II", "III" , "IV", "V"}; // Set default values for buffTiers
        buffBonus = new float[] { 0.5f, 1.0f, 1.5f , 2.0f , 2.5f}; // Set default values for buffBonus
    }

    public void ResetAtkSpd()
    {
        buffBonus = new float[] { 0.5f, 1.0f, 1.5f , 2.0f , 2.5f};
    }
}
