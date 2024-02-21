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
            GameObject kunai = ObjectPoolManager.Instance.SpawnObject(abilityProjectile, firePoint.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
            kunai.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward;

            SetDirection(kunai);

            kunai.GetComponent<GameObjectProjectile>().MoveProjectile();   

            abilityCooldownTimer = rangedWeaponData.abilitycooldown;
        }
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {
            GameObject middleShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
            middleShuriken.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward;
            middleShuriken.GetComponent<GameObjectProjectile>().MoveProjectile();


            GameObject leftShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
            leftShuriken.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward - camRotation.right;
            leftShuriken.GetComponent<GameObjectProjectile>().MoveProjectile();


            GameObject rightShuriken = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
            rightShuriken.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward + camRotation.right;
            rightShuriken.GetComponent<GameObjectProjectile>().MoveProjectile();

            secondaryCooldownTimer = rangedWeaponData.secondarycooldown;
        }
    }

    protected override void OnSecondary()
    {
    }

    protected override void OnAbility()
    {
    }
}
