using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private GameObject selectedBorder;
    private GridLayoutGroup invGridLayoutGroup;
    [SerializeField] private GameObject invGridElementPrefab;
    [SerializeField] private LayerMask targetableLayer;
    [SerializeField] private int invSlots;
    private Transform cam;

    [SerializeField] private List<Sprite> weaponIcons = new List<Sprite>();
    public static GameObject selectBorder;
    public static List<Sprite> invWeaponIcons = new List<Sprite>();
    public static List<GameObject> invWeapons = new List<GameObject>();
    public static List<GameObject> invUISlots = new List<GameObject>();

    public static int maxInvSlots;
    public static int selectedWeaponIndex = 0;
    public static int currWeaponIndex = 0;
    public static int prevWeaponIndex = 0;

    private static bool swapWeapon = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
    }

    public void SetInventory()
    {
        if (SceneManager.GetActiveScene().name == "LevelScene")
        {
            selectBorder = null;
            invWeaponIcons = new List<Sprite>();
            invWeapons = new List<GameObject>();
            invUISlots = new List<GameObject>();

            maxInvSlots = 0;
            selectedWeaponIndex = 0;
            currWeaponIndex = 0;
            prevWeaponIndex = 0;

            if (invSlots >= maxInvSlots)
            {
                maxInvSlots = invSlots;
            }

            int newWidth = 150 * maxInvSlots;
            selectedBorder = GameObject.FindGameObjectWithTag("SelectedBorder");
            invGridLayoutGroup = GameObject.FindGameObjectWithTag("InventoryPanel").GetComponent<GridLayoutGroup>();
            cam = GameObject.FindGameObjectWithTag("CameraHolder").transform;
            RectTransform invGridLayoutGroupTransform = invGridLayoutGroup.gameObject.GetComponent<RectTransform>();
            invGridLayoutGroupTransform.sizeDelta = new Vector2(newWidth, 150);

            for (int i = 0; i < maxInvSlots; i++)
            {
                GameObject newSlot = Instantiate(invGridElementPrefab);
                newSlot.transform.SetParent(invGridLayoutGroup.transform);
                newSlot.transform.localScale = new Vector3(1, 1, 1);

                invUISlots.Add(newSlot);
            }
            Debug.Log(maxInvSlots);
            invWeaponIcons = weaponIcons;
            selectBorder = selectedBorder;
            selectedBorder.transform.localPosition = invUISlots[0].transform.localPosition;
        }
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "LevelScene")
        {
            return;
        }

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

                if (invWeapons[currWeaponIndex].TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
                {
                    if (rangedWeapon.rangedWeaponData.infiniteAmmo)
                    {
                        rangedWeapon.ClearText();
                    }
                    else
                    {
                        rangedWeapon.UpdateAmmo();
                    }
                }

                selectedWeaponIndex = invWeapons[currWeaponIndex].GetComponent<Weapon>().inventoryPosition;
                selectedBorder.transform.localPosition = invUISlots[selectedWeaponIndex].transform.localPosition;

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

                if (invWeapons[currWeaponIndex].TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
                {
                    if (rangedWeapon.rangedWeaponData.infiniteAmmo)
                    {
                        rangedWeapon.ClearText();
                    }
                    else
                    {
                        rangedWeapon.UpdateAmmo();
                    }
                }

                selectedWeaponIndex = invWeapons[currWeaponIndex].GetComponent<Weapon>().inventoryPosition;
                selectedBorder.transform.localPosition = invUISlots[selectedWeaponIndex].transform.localPosition;

                // Set can attack here
                // Can set get weapon stats here too if need be
            }
        }
    }

    private void InputDropWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedWeaponIndex = invWeapons[currWeaponIndex].GetComponent<Weapon>().inventoryPosition;
            GameObject.FindWithTag("Player").GetComponent<PlayerWeaponDrop>().DropWeapon(invWeapons[currWeaponIndex]);
            invUISlots[selectedWeaponIndex].GetComponent<Image>().sprite = null;

            if (currWeaponIndex != 0)
            {
                currWeaponIndex--;
            }
            
            if (invWeapons.Count > 0)
            {
                invWeapons[currWeaponIndex].SetActive(true);

                if (invWeapons[currWeaponIndex].TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
                {
                    if (rangedWeapon.rangedWeaponData.infiniteAmmo)
                    {
                        rangedWeapon.ClearText();
                    }
                    else
                    {
                        rangedWeapon.UpdateAmmo();
                    }
                }

                selectedWeaponIndex = invWeapons[currWeaponIndex].GetComponent<Weapon>().inventoryPosition;
                selectedBorder.transform.localPosition = invUISlots[selectedWeaponIndex].transform.localPosition;
            }
        }
    }

    private void InputSwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F) && invWeapons.Count == maxInvSlots)
        {
            Vector3 direction = cam.forward;

            GameObject hitObj = GetLOSObject(direction, false, targetableLayer);

            if (hitObj != null && !swapWeapon)
            {
                swapWeapon = true;

                GameObject.FindWithTag("Player").GetComponent<PlayerWeaponDrop>().DropWeapon(invWeapons[currWeaponIndex]);
                GetComponent<PlayerWeaponPickup>().PickUpWeapon(hitObj);
                
                invWeapons[currWeaponIndex].SetActive(true);

                if (invWeapons[currWeaponIndex].TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
                {
                    if (rangedWeapon.rangedWeaponData.infiniteAmmo)
                    {
                        rangedWeapon.ClearText();
                    }
                    else
                    {
                        rangedWeapon.UpdateAmmo();
                    }
                }

                selectedWeaponIndex = invWeapons[currWeaponIndex].GetComponent<Weapon>().inventoryPosition;
                selectedBorder.transform.localPosition = invUISlots[selectedWeaponIndex].transform.localPosition;
            }
        }
    }

    private GameObject GetLOSObject(Vector3 dir, bool ignoreLayers, LayerMask mask)
    {
        if (!ignoreLayers)
        {
            if (Physics.Raycast(cam.position, dir, out RaycastHit hit, 1000, mask))
            {
                return hit.collider.gameObject;
            }
        }
        else
        {
            if (Physics.Raycast(cam.position, dir, out RaycastHit hit, 1000, ~mask))
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
                weapon.GetComponent<Weapon>().inventoryPosition = currWeaponIndex;

                invUISlots[currWeaponIndex].GetComponent<Image>().sprite = invWeaponIcons[(int)weapon.GetComponent<Weapon>().type];
                selectBorder.transform.localPosition = invUISlots[currWeaponIndex].transform.localPosition;

                if (invWeapons[currWeaponIndex].TryGetComponent<RangedWeapon>(out RangedWeapon rangedWeapon))
                {
                    if (rangedWeapon.rangedWeaponData.infiniteAmmo)
                    {
                        rangedWeapon.ClearText();
                    }
                    else
                    {
                        rangedWeapon.UpdateAmmo();
                    }
                }

                Debug.Log("Swap Weapon : " + weapon.name);
                swapWeapon = false;
            }
            else
            {
                invWeapons.Add(weapon);
                
                for (int i = 0; i < invWeapons.Count; i++)
                {
                    if (invUISlots[i].GetComponent<Image>().sprite == null)
                    {
                        weapon.GetComponent<Weapon>().inventoryPosition = i;

                        invUISlots[i].GetComponent<Image>().sprite = invWeaponIcons[(int)weapon.GetComponent<Weapon>().type];

                        break;
                    }
                }

                Debug.Log("Added Weapon : " + weapon.name);
            }

            if (invWeapons.Count == 1)
            {
                currWeaponIndex = 0;
                weapon.SetActive(true);
                selectBorder.transform.localPosition = invUISlots[currWeaponIndex].transform.localPosition;
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
