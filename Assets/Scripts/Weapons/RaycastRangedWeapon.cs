using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastRangedWeapon : RangedWeapon
{
    [SerializeField] private RaycastProjectileData raycastProjectileData;
    [SerializeField] private LineRenderer lineRenderer;

    public override void Shoot()
    {
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanShoot())
            {
                // Debug.Log("Shoot");
                if (Physics.Raycast(cam.position, transform.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance))
                {
                    StartCoroutine(RenderTraceLine(hitInfo.point));
                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(raycastProjectileData.damage);
                    
                    // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
                    // Destroy(effect, 0.5f);
                }
                else
                {
                    StartCoroutine(RenderTraceLine(cam.transform.forward * raycastProjectileData.maxDistance));
                }

                if (!rangedWeaponData.infiniteAmmo)
                rangedWeaponData.currentAmmo--;

                timeSinceLastShot = 0;
                OnShot();
                // recoil.GunRecoil(gunData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
            }
        }
    }

    public override void OnShot()
    {
    }

    private IEnumerator RenderTraceLine(Vector3 hitPosition)
    {
        // audController.PlayAudio("shoot");
        
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.1f);

        lineRenderer.positionCount = 0;
    }
}
