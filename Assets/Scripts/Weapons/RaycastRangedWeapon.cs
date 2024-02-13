using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastRangedWeapon : RangedWeapon
{
    [SerializeField] private RaycastProjectileData raycastProjectileData;

    public override void OnShot()
    {
        timeSinceLastShot += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
    }

    public override void Shoot()
    {
        if (rangedWeaponData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cam.position, transform.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance))
                {
                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(raycastProjectileData.damage);
                    
                    // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
                    // Destroy(effect, 0.5f);
                }

                rangedWeaponData.currentAmmo--;
                timeSinceLastShot = 0;
                OnShot();
                // recoil.GunRecoil(gunData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
            }
        }
    }

    public override void Update()
    {
    }
}
