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
            kunai.GetComponent<GameObjectProjectile>().projectileDirection = cam.forward;

            SetDirection(kunai);

            kunai.GetComponent<GameObjectProjectile>().MoveProjectile();   

            abilityCooldownTimer = rangedWeaponData.abilitycooldown;
        }
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {
            GameObject middleShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            middleShuriken.GetComponent<GameObjectProjectile>().projectileDirection = cam.forward;
            middleShuriken.GetComponent<GameObjectProjectile>().MoveProjectile();


            GameObject leftShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            leftShuriken.GetComponent<GameObjectProjectile>().projectileDirection = cam.forward - cam.right;
            leftShuriken.GetComponent<GameObjectProjectile>().MoveProjectile();


            GameObject rightShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
            rightShuriken.GetComponent<GameObjectProjectile>().projectileDirection = cam.forward + cam.right;
            rightShuriken.GetComponent<GameObjectProjectile>().MoveProjectile();

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
