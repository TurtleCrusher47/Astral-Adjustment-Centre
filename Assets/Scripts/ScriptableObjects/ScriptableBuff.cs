using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableBuff : ScriptableObject
{
    public string buffName;

    public int currBuffTier = 0;

    [SerializeField] public string[] buffTiers = new string[] { "I", "II", "III", "IV", "V" };

    [SerializeField] public float[] buffBonus = new float[] { 1.1f, 1.2f, 1.3f, 1.4f, 1.5f };

    // Reset  Buff
    public void ResetBuffTier()
    {
        buffTiers[0] = "I";
        currBuffTier = 0;
    }
}
