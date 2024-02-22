using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RaycastRangedWeapon : RangedWeapon
{
    [SerializeField] protected RaycastProjectileData raycastProjectileData;
    [SerializeField] protected LineRenderer lineRenderer;

    protected override void UsePrimary()
    {
        animator.ResetTrigger("Primary");
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanShoot())
            {
                // Debug.Log("Shoot");
                if (Physics.Raycast(camRotation.position, camRotation.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance, targetLayers))
                {
                    StartCoroutine(RenderTraceLine(hitInfo.point));
                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(raycastProjectileData.damage * BuffManager.Instance.buffs[1].buffBonus[BuffManager.Instance.buffs[1].currBuffTier]);
                    
                    // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
                    // Destroy(effect, 0.5f);
                }
                else
                {
                    StartCoroutine(RenderTraceLine(camRotation.forward.normalized * raycastProjectileData.maxDistance));
                }

                if (!rangedWeaponData.infiniteAmmo)
                rangedWeaponData.currentAmmo--;

                timeSinceLastShot = 0;
                //recoil.GunRecoil(rangedWeaponData.recoil);
                OnPrimary();
                // recoil.GunRecoil(gunData.recoil);

                // updateAmmoText.UpdateAmmo(gunData.currentAmmo, gunData.magazineSize);
            }
        }
    }

    protected override void OnPrimary()
    {
        animator.SetTrigger("Primary");
    }

    protected IEnumerator RenderTraceLine(Vector3 hitPosition, float renderedTime = 0.1f)
    {
        // audController.PlayAudio("shoot");
        
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(renderedTime);

        lineRenderer.positionCount = 0;
    }
}
