using System.Collections;
using System.Collections.Generic;
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

            if (weapon.CompareTag("BaseSword"))
            {
                weapon.transform.localPosition = new Vector3(0, 0, 0);
                weapon.transform.localRotation = Quaternion.Euler(0, -90, 0);

            }
            else if (weapon.CompareTag("RayGun"))
            {
                weapon.transform.localPosition = new Vector3(0, 0, 0);
                weapon.transform.localRotation = Quaternion.Euler(-90, 0, -90);
            }

            weapon.GetComponent<Weapon>().inInventory = true;
            weapon.GetComponent<Rigidbody>().isKinematic = true;

            canPickUp = false;
            weapon.layer = LayerMask.NameToLayer("Weapons");
        }
    }
}
