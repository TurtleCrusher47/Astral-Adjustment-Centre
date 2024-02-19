using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/AtkSpdBuff")]
public class ScriptableAtkSpdBuff : ScriptableBuff
{
    public float AtkSpdIncrease;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedSpeedBuff(this, obj);
    }
}
