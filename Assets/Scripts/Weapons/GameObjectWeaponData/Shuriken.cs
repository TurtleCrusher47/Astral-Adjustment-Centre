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
            GameObject middleShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            middleShuriken.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward;
            middleShuriken.GetComponent<ShurikenProjectile>().MoveProjectile();

            GameObject leftShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            leftShuriken.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward - cam.right;
            leftShuriken.GetComponent<ShurikenProjectile>().MoveProjectile();

            GameObject rightShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            rightShuriken.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward + cam.right;
            rightShuriken.GetComponent<ShurikenProjectile>().MoveProjectile();
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
