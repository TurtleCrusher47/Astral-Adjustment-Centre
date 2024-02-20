using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectRangedWeapon : RangedWeapon
{
    [SerializeField] protected GameObjectProjectileData gameObjectProjectileData;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected Animator animator;

    protected override void UsePrimary()
    {
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanShoot())
            {
                // Debug.Log("Shot GameObject");
                GameObject currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint.position, cam.rotation, ObjectPoolManager.PoolType.Projectile);
                currentProjectile.GetComponent<GameObjectProjectile>().projectileDirection = cam.forward;

                SetDirection(currentProjectile);

                currentProjectile.GetComponent<GameObjectProjectile>().MoveProjectile();

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
        animator.SetTrigger("Primary");
    }

    protected void SetDirection(GameObject projectile)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, gameObjectProjectileData.range, targetLayers))
        {
            Debug.Log("Ray " + hit.collider.gameObject.name);
            projectile.GetComponent<GameObjectProjectile>().projectileDirection = (hit.point - firePoint.position).normalized;
        }
    }
}
