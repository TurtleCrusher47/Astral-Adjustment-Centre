using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] public TrapData trapData;
    [SerializeField] protected LayerMask collidableLayers;

    protected ScriptableBuff atkBuff;
    protected float atkBuffMultiplier;

    protected bool isTriggered;

    private void OnEnable()
    {
        isTriggered = false;
    }

    protected float GetAtkMultiplier()
    {
        atkBuff = BuffManager.Instance.buffs[0];
        if (atkBuff.currBuffTier > 0)
        {
            atkBuffMultiplier = atkBuff.buffBonus[atkBuff.currBuffTier - 1];
        }
        else
        {
            atkBuffMultiplier = 1;
        }
        return atkBuffMultiplier;
    }

    protected abstract void OnTriggerEnter(Collider collider);

    public abstract IEnumerator TriggerTrap();

    // Effects + SFX
    protected abstract void OnTrapTriggered();
}
