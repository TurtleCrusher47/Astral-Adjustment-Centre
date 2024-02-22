using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Projectile/GameObject")]
public class GameObjectProjectileData : ScriptableObject
{
    [Header("Shooting")]    
    public float damage;
    public float force;
    public float range;
    public float timeBeforeDestroy;
}
