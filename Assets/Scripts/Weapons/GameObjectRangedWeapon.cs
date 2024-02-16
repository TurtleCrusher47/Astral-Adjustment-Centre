using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectRangedWeapon : RangedWeapon
{
    [SerializeField] protected GameObjectProjectileData gameObjectProjectileData;
    [SerializeField] protected GameObject projectile;

    protected GameObject currentProjectile;

    protected override void UsePrimary()
    {
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanShoot())
            {
                // Debug.Log("Shot GameObject");
                currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, transform.rotation, ObjectPoolManager.PoolType.Ammo);

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
