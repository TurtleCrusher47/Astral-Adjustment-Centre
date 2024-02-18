using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDrop : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform cam;

    [SerializeField] private float dropForwardForce, dropUpwardForce;

    private Rigidbody rb;

    void Awake()
    {
        player = gameObject;
        //cam = Camera.main;
    }

    public void DropWeapon(GameObject weapon)
    {
        PlayerInventory.DropWeapon(weapon);

        weapon.transform.parent = null;

        weapon.GetComponent<Weapon>().enabled = false;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        rb = weapon.GetComponent<Rigidbody>();

        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse);

        float random = Random.Range(-1, 1);

        rb.AddTorque(new Vector3(random, random, random) * 10);

        StartCoroutine(AllowPickUp(weapon));

        foreach (Transform child in weapon.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("DroppedWeapons");
        }
    }

    private IEnumerator AllowPickUp(GameObject weapon)
    {
        yield return new WaitForSeconds(1);
        
        weapon.GetComponent<Weapon>().inInventory = false;
    }
}
