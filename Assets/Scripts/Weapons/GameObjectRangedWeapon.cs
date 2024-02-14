using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRangedWeapon : RangedWeapon
{
    [SerializeField] protected GameObjectProjectileData gameObjectProjectileData;
    [SerializeField] protected GameObject projectile;

    protected GameObject currentProjectile;

    public override void OnShot()
    {
    }

    public override void Shoot()
    {
        if (rangedWeaponData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                // Debug.Log("Shot GameObject");
                currentProjectile = ObjectPoolManager.SpawnObject(projectile, firePoint.position, transform.rotation, ObjectPoolManager.PoolType.Ammo);

                rangedWeaponData.currentAmmo--;
                timeSinceLastShot = 0;
                // recoil.GunRecoil(gunData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
                OnShot();
            }
        }
    }
}
