using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AttackBuff")]
public class ScriptableAttackBuff : ScriptableBuff
{
    public float AttackIncrease;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedSpeedBuff(this, obj);
    }
}