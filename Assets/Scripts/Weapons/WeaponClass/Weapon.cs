using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [HideInInspector]
    public enum Type
    {
        MELEE,
        RANGED
    }

    [HideInInspector] public Type type;

    [HideInInspector] public bool inInventory;
    [HideInInspector] public int inventoryPosition;

    protected float secondaryCooldownTimer;
    protected float abilityCooldownTimer;

    // To init the weapon type
    protected abstract void Start();

    // For weapon pickup
    protected abstract void OnTriggerEnter(Collider other);

    // Left Click
    protected abstract void UsePrimary();

    // Left Click Up
    protected virtual void UseSecondaryUp()
    {

    }

    // Right Click
    protected abstract void UseSecondary();

    // E
    protected abstract void UseAbility();

    // Checks
    protected bool CanUseSecondary()
    {
        return secondaryCooldownTimer <= 0;
    }

    protected bool CanUseAbility()
    {
        return abilityCooldownTimer <= 0;
    }

    // Effects
    protected abstract void OnPrimary();

    protected abstract void OnSecondary();

    protected abstract void OnAbility();
}