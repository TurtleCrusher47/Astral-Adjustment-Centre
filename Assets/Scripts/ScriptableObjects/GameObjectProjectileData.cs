using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Projectile/GameObject")]
public class GameObjectProjectileData : ScriptableObject
{
    [Header("Shooting")]    
    public int damage;
    public int force;
    public int range;
    public float timeBeforeDestroy;
}
