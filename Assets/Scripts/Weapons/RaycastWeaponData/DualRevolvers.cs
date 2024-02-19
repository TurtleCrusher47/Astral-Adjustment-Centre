using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualRevolvers : RaycastRangedWeapon
{
    [SerializeField] Transform firePoint2;

    protected override void OnSecondary()
    {
        
    }

    protected override void OnSkill()
    {
        throw new System.NotImplementedException();
    }

    protected override void UseAbility()
    {
        throw new System.NotImplementedException();
    }

    protected override void UseSecondary()
    {
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanUseSecondary())
            {
                // Debug.Log("Shoot");
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance, ~layersToIgnore))
                {
                    StartCoroutine(RenderSecondTraceLine(hitInfo.point));
                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(raycastProjectileData.damage);
                    
                    // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
                    // Destroy(effect, 0.5f);
                }
                else
                {
                    StartCoroutine(RenderSecondTraceLine(cam.forward * raycastProjectileData.maxDistance));
                }

                if (!rangedWeaponData.infiniteAmmo)
                rangedWeaponData.currentAmmo--;

                secondaryCooldownTimer = rangedWeaponData.secondarycooldown;

                OnSecondary();
                // recoil.GunRecoil(gunData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
            }
        }
    }

    private IEnumerator RenderSecondTraceLine(Vector3 hitPosition)
    {
        // audController.PlayAudio("shoot");
        
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint2.position);
        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.1f);

        lineRenderer.positionCount = 0;
    }
}
