using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged", menuName = "Weapon/Ranged")]
public class RangedWeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Reloading")]
    public bool infiniteAmmo;
    public int currentAmmo;
    public int magazineSize;
    [Tooltip("The higher this is, the faster it can fire")]
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;

    [Header("Recoil")]
    public Vector3 recoil;
}
