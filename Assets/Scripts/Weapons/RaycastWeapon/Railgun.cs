using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Railgun : RaycastRangedWeapon
{
    private float chargeMultiplier = 1.25f;
    private float elapsedTime = 0;

    protected override void UseSecondary()
    {
        // elapsed time to check per second
        if (rangedWeaponData.currentAmmo - 10 >= 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1f)
            {
                elapsedTime %= 1f;
                chargeMultiplier += 1;
                rangedWeaponData.currentAmmo -= 10;
            }
        }
        // If player starts reloading while charging
        if (rangedWeaponData.reloading)
        {
            ResetValues();
        }
    }

    protected override void UseSecondaryUp()
    {
        // To store the variable that was furthest
        RaycastHit furthestHit;

        // Do not do damage if the player did not properly charge
        if (chargeMultiplier <= 1.25f)
        {
            ResetValues();
            return;
        }
        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, raycastProjectileData.maxDistance, targetLayers);
        // Assign furthestHit to the first point in hits
        furthestHit = hits[0];
        for (int i = 0; i < hits.Length; i++)
        {
            // If the new point is further away than the old point
            if (Vector3.Distance(firePoint.position, hits[i].point) >= Vector3.Distance(firePoint.position, furthestHit.point))
            {
                furthestHit = hits[i];
            }

            if (hits[i].collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(raycastProjectileData.damage * chargeMultiplier);
            }
            Debug.Log(hits[i].transform.name);

            // If it is the last object
            if (i == hits.Length - 1)
            {
                StartCoroutine(RenderTraceLine(furthestHit.point, 1));
            }
        }

        ResetValues();

        // Debug.Log("RB Up");
        OnSecondary();
    }

    private void ResetValues()
    {
        elapsedTime = 0;
        chargeMultiplier = 1.25f;
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