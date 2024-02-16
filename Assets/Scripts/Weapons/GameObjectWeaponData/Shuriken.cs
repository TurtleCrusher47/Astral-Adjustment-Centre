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
            GameObject middleShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, transform.rotation, ObjectPoolManager.PoolType.Ammo);
            middleShuriken.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward;

            GameObject leftShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, transform.rotation, ObjectPoolManager.PoolType.Ammo);
            leftShuriken.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward - cam.right;

            GameObject rightShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, transform.rotation, ObjectPoolManager.PoolType.Ammo);
            rightShuriken.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward + cam.right;
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
