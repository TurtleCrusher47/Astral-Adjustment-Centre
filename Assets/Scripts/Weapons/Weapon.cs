using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        MELEE,
        RANGE
    }

    //[SerializeField] public ScriptableWeapon weaponStats;
    public bool inInventory;
    public Type type;
    public int invPos;

    void Awake()
    {
        // For gun can initialise ammo etc or something
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider") && !inInventory)
        {
            other.transform.parent.GetComponent<PlayerWeaponPickup>().PickUpWeapon(gameObject);
        }
    }
}
