using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectRangedWeapon : RangedWeapon
{
    [SerializeField] protected GameObjectProjectileData gameObjectProjectileData;
    [SerializeField] protected GameObject projectile;

    protected override void UsePrimary()
    {
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanShoot())
            {
                // Debug.Log("Shot GameObject");
                GameObject currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
                currentProjectile.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward;

                SetDirection(currentProjectile);

                currentProjectile.GetComponent<GameObjectProjectile>().MoveProjectile();

                // Only deduct ammo if the ammo is not infinite
                if (!rangedWeaponData.infiniteAmmo)
                {
                    rangedWeaponData.currentAmmo--;
                    UpdateAmmo();
                }

                timeSinceLastShot = 0;
                recoil.GunRecoil(rangedWeaponData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
                OnPrimary();
            }
        }
    }

    protected override void OnPrimary()
    {
        animator.SetTrigger("Primary");
    }

    protected void SetDirection(GameObject projectile)
    {
        RaycastHit[] hits = Physics.RaycastAll(cam.position, camRotation.forward, gameObjectProjectileData.range, targetLayers);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer == targetLayers)
            projectile.GetComponent<GameObjectProjectile>().projectileDirection = (hits[i].point - firePoint.position).normalized;
        }
    }
}
