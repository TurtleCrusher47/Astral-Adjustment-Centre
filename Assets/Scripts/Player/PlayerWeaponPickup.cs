using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponContainer;
    private bool canPickUp;

    public void PickUpWeapon(GameObject weapon)
    {
        canPickUp = PlayerInventory.CheckWeapon(weapon);

        if (canPickUp)
        {
            weapon.transform.SetParent(weaponContainer.transform);

            weapon.transform.localPosition = new Vector3(0, 0, 0);
            weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);

            weapon.GetComponent<Weapon>().inInventory = true;
            weapon.GetComponent<Weapon>().enabled = true;
            weapon.GetComponent<Rigidbody>().isKinematic = true;

            canPickUp = false;

            foreach (Transform child in weapon.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Weapons");
            }
        }
    }
}
