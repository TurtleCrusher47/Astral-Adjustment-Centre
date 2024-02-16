using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : GameObjectRangedWeapon
{
    protected override void UseAbility()
    {
        if (CanUseAbility())
        {

        }
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {
            // currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, transform.rotation, ObjectPoolManager.PoolType.Ammo);

        }
    }

    protected override void OnSecondary()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSkill()
    {
        throw new System.NotImplementedException();
    }
}
