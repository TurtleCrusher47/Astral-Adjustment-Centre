using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/FireRateBuff")]
public class ScriptableFireRateBuff : ScriptableBuff
{
    public float FireRateIncrease;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedSpeedBuff(this, obj);
    }
}
