using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee", menuName = "Weapon/Melee")]
public class MeleeWeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Tooltip("The higher this is, the faster it can fire")]
    [Header("Attack")]
    public float attackRate;
    public float damage;

    [Header("Skills")]
    public float secondarycooldown;
    public float abilitycooldown;
}
