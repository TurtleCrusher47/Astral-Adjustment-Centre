using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] public TrapData trapData;
    [SerializeField] protected LayerMask collidableLayers;

    private bool isTriggered;

    private void OnEnable()
    {
        isTriggered = false;
    }

    protected abstract void OnTriggerEnter(Collider collider);

    protected abstract void TriggerTrap();

    // Effects + SFX
    protected abstract void OnTrapTriggered();
}
