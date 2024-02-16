using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : GameObjectRangedWeapon
{
    [SerializeField] GameObject abilityProjectile;

    protected override void UseAbility()
    {
        if (CanUseAbility())
        {
            GameObject kunai = ObjectPoolManager.Instance.SpawnObject(abilityProjectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            kunai.GetComponent<ShurikenProjectile>().projectileDirection = cam.forward;
            kunai.GetComponent<ShurikenProjectile>().MoveProjectile();   

            abilityCooldownTimer = rangedWeaponData.abilitycooldown;
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

            secondaryCooldownTimer = rangedWeaponData.secondarycooldown;
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
