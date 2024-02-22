using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxe : GameObjectRangedWeapon
{
    [SerializeField] Transform firePoint2;

    protected override void OnAbility()
    {
    }

    protected override void OnSecondary()
    {
    }

    protected override void UseAbility()
    {
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {
            // Debug.Log("Shot GameObject");
            GameObject currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint2.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
            currentProjectile.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward;

            SetDirectionSecondary(currentProjectile);

            currentProjectile.GetComponent<GameObjectProjectile>().MoveProjectile();

            secondaryCooldownTimer = rangedWeaponData.secondarycooldown;
            OnSecondary();
        }
    }

    protected void SetDirectionSecondary(GameObject projectile)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, camRotation.forward, out hit, gameObjectProjectileData.range, targetLayers))
        {
            Debug.Log("Ray " + hit.collider.gameObject.name);
            projectile.GetComponent<GameObjectProjectile>().projectileDirection = (hit.point - firePoint2.position).normalized;
        }
    }
}
