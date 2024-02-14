using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Projectile/Raycast")]
public class RaycastProjectileData : ScriptableObject
{
    [Header("Shooting")]
    public int damage;
    public float maxDistance;
}
