using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Projectile/Trap")]
public class TrapData : ScriptableObject
{
    [Header("Trap")]
    public int damage;
    public float damageRange;
    public float timeBeforeDestroy;
    public float triggerRange;
    public float triggerDelay;
}
