using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableBuff : ScriptableObject
{
    public string buffName;

    public string[] buffTiers;

    public float[] buffBonus;
}
