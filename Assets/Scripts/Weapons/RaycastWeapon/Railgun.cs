using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : RaycastRangedWeapon
{
    private float chargeMultiplier = 0.25f;

    protected override void UseSecondary()
    {
        if (rangedWeaponData.currentAmmo - 10 >= 0)
        {
            chargeMultiplier += Time.deltaTime;
            rangedWeaponData.currentAmmo -= 10;
        }
    }

    protected override void UseSecondaryUp()
    {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance, targetLayers))
        {
            StartCoroutine(RenderTraceLine(hitInfo.point));
            Debug.Log(hitInfo.transform.name);
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable?.Damage(raycastProjectileData.damage * chargeMultiplier);
            
            // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
            // Destroy(effect, 0.5f);
        }
        else
        {
            StartCoroutine(RenderTraceLine(cam.forward * raycastProjectileData.maxDistance));
        }

        // Debug.Log("RB Up");
        OnSecondary();
    }

    protected override void UseAbility()
    {
    }

    protected override void OnSecondary()
    {
    }

    protected override void OnAbility()
    {
    }
}
