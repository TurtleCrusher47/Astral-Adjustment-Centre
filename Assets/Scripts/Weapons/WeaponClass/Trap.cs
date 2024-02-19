using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    [SerializeField] public TrapData trapData;
    [SerializeField] protected LayerMask collidableLayers;

    protected bool isTriggered;

    private void OnEnable()
    {
        isTriggered = false;
    }

    protected abstract void OnTriggerEnter(Collider collider);

    public abstract IEnumerator TriggerTrap();

    // Effects + SFX
    protected abstract void OnTrapTriggered();
}
