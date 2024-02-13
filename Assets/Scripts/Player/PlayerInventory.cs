using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int invSlots;
    [SerializeField] private LayerMask targetableLayer;

    public static List<GameObject> invWeapons = new List<GameObject>();

    public static int maxInvSlots = 1;
    public static int currWeaponIndex = 0;
    public static int prevWeaponIndex = 0;

    private static bool swapWeapon = false;

    void Awake()
    {
        if (invSlots > maxInvSlots)
        {
            maxInvSlots = invSlots;
        }
    }

    void Update()
    {
        InputChangeWeapon();
        InputDropWeapon();
        InputSwapWeapon();
    }

    private void InputChangeWeapon()
    {
        if (invWeapons.Count > 1)
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {

                if (currWeaponIndex >= invWeapons.Count - 1)
                {
                    prevWeaponIndex = currWeaponIndex;
                    currWeaponIndex = 0;
                }
                else
                {
                    prevWeaponIndex = currWeaponIndex;
                    currWeaponIndex++;
                }
                
                invWeapons[prevWeaponIndex].SetActive(false);
                invWeapons[currWeaponIndex].SetActive(true);

                // Set can attack here
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                if (currWeaponIndex <= 0)
                {
                    prevWeaponIndex = currWeaponIndex;
                    currWeaponIndex = invWeapons.Count - 1;
                }
                else
                {
                    prevWeaponIndex = currWeaponIndex;
                    currWeaponIndex--;
                }
                
                invWeapons[prevWeaponIndex].SetActive(false);
                invWeapons[currWeaponIndex].SetActive(true);

                // Set can attack here
                // Can set get weapon stats here too if need be
            }
        }
    }

    private void InputDropWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            /*isReloading = false;
            CancelInvoke("ReloadFinished");

            if (isADSing)
            {
                isADSing = false;
                NotifyADSObservers(gunModels[currWeaponIndex], false);
            }
            
            NotifyUpdateWeaponObservers(false, false);

            IADSObserver currADS = gunModels[currWeaponIndex].GetComponent<IADSObserver>();
            UnsubscribeADS(currADS);

            IShootObserver currShoot = gunModels[currWeaponIndex].GetComponent<IShootObserver>();
            UnsubscribeShoot(currShoot);*/

            GameObject.FindWithTag("Player").GetComponent<PlayerWeaponDrop>().DropWeapon(invWeapons[currWeaponIndex]);
            
            if (currWeaponIndex != 0)
            {
                currWeaponIndex--;
            }

            if (invWeapons.Count > 0)
            {
                invWeapons[currWeaponIndex].SetActive(true);
                //GetcurrWeaponIndexStats();
            }
        }
    }

    private void InputSwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F) && invWeapons.Count == maxInvSlots)
        {
            Vector3 direction = Camera.main.transform.forward;

            GameObject hitObj = GetLOSObject(direction, false, targetableLayer);

            if (hitObj != null && !swapWeapon)
            {
                swapWeapon = true;
                GameObject.FindWithTag("Player").GetComponent<PlayerWeaponDrop>().DropWeapon(invWeapons[currWeaponIndex]);
                GetComponent<PlayerWeaponPickup>().PickUpWeapon(hitObj);
                invWeapons[currWeaponIndex].SetActive(true);
            }
        }
    }

    private GameObject GetLOSObject(Vector3 dir, bool ignoreLayers, LayerMask mask)
    {
        if (!ignoreLayers)
        {
            if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hit, 1000, mask))
            {
                return hit.collider.gameObject;
            }
        }
        else
        {
            if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hit, 1000, ~mask))
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    public static bool CheckWeapon(GameObject weapon)
    {
        if (invWeapons.Count < maxInvSlots)
        {
            if (swapWeapon)
            {
                invWeapons.Insert(currWeaponIndex, weapon);
                Debug.Log("Swap Weapon : " + weapon.name);
                swapWeapon = false;
            }
            else
            {
                invWeapons.Add(weapon);
                Debug.Log("Added Weapon : " + weapon.name);
            }

            if (invWeapons.Count == 1)
            {
                currWeaponIndex = 0;
                weapon.SetActive(true);
            }
            else
            {
                weapon.SetActive(false);
            }

            return true;
        }
        
        return false;
    }

    public static void DropWeapon(GameObject selected)
    {
        invWeapons.Remove(selected);
    }
}
