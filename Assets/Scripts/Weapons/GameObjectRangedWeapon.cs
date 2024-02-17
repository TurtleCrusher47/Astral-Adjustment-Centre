using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectRangedWeapon : RangedWeapon
{
    [SerializeField] protected GameObjectProjectileData gameObjectProjectileData;
    [SerializeField] protected GameObject projectile;

    [SerializeField] protected LayerMask weaponLayer;


    protected override void UsePrimary()
    {
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanShoot())
            {
                // Debug.Log("Shot GameObject");
                GameObject currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
                currentProjectile.GetComponent<GameObjectProjectile>().projectileDirection = cam.forward;

                RaycastHit hit;
                if (Physics.Raycast(cam.position, cam.forward, out hit, gameObjectProjectileData.range, ~weaponLayer))
                {
                    Debug.Log("Ray " + hit.collider.gameObject.name);
                    currentProjectile.GetComponent<GameObjectProjectile>().projectileDirection = (hit.point - firePoint.position).normalized;

                }

                currentProjectile.GetComponent<ShurikenProjectile>().MoveProjectile();

                // Only deduct ammo if the ammo is not infinite
                if (!rangedWeaponData.infiniteAmmo)
                rangedWeaponData.currentAmmo--;

                timeSinceLastShot = 0;
                // recoil.GunRecoil(gunData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
                OnPrimary();
            }
        }
    }

    protected override void OnPrimary()
    {
    }
}
