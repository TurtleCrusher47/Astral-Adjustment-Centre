using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Type
    {
        MELEE,
        RANGED
    }
    public Type type;

    public bool inInventory;
    public int inventoryPosition;

    // To init the weapon type
    protected abstract void Init();

    // For weapon pickup
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider") && !inInventory)
        {
            other.transform.parent.GetComponent<PlayerWeaponPickup>().PickUpWeapon(gameObject);
        }
    }
}