using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualRevolvers : RaycastRangedWeapon
{
    [SerializeField] Transform firePoint2;
    [SerializeField] protected LineRenderer lineRenderer2;

    protected override void OnPrimary()
    {
        animator.SetTrigger("Primary");
        AudioManager.Instance.PlaySFX("SFXRevolverShoot");
    }

    protected override void OnSecondary()
    {
        animator.SetTrigger("Secondary");
        AudioManager.Instance.PlaySFX("SFXRevolverShoot");
    }

    protected override void OnAbility()
    {
    }

    protected override void UseAbility()
    {
    }

    protected override void UseSecondary()
    {
        animator.ResetTrigger("Secondary");
        if (rangedWeaponData.currentAmmo > 0 || rangedWeaponData.infiniteAmmo)
        {
            if (CanUseSecondary())
            {
                // Debug.Log("Shoot");
                if (Physics.Raycast(cam.position, camRotation.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance, targetLayers))
                {
                    StartCoroutine(RenderSecondTraceLine(hitInfo.point));
                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(raycastProjectileData.damage * GetAtkMultiplier());
                    
                    // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
                    // Destroy(effect, 0.5f);
                }
                else
                {
                    StartCoroutine(RenderSecondTraceLine(cam.forward * raycastProjectileData.maxDistance));
                }

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
        
        lineRenderer2.positionCount = 2;
        lineRenderer2.SetPosition(0, firePoint2.position);
        lineRenderer2.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.1f);

        lineRenderer2.positionCount = 0;
    }
}
