using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Railgun : RaycastRangedWeapon
{
    private float chargeMultiplier = 1.25f;
    private float elapsedTime = 0;
    [SerializeField] private VisualEffect chargeUpEffect;

    private void Awake()
    {
        chargeUpEffect.Stop();
    }

    private void OnEnable()
    {
        chargeUpEffect.Stop();
    }

    protected override void UseSecondary()
    {
        // elapsed time to check per second
        if (rangedWeaponData.currentAmmo - 10 >= 0)
        {
            chargeUpEffect.Play();
            elapsedTime += Time.deltaTime;
            AudioManager.Instance.PlaySFXLoop("SFXChargeUp");
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
        chargeUpEffect.Stop();
        // To store the variable that was furthest
        RaycastHit furthestHit;

        AudioManager.Instance.StopSFXLoop("SFXChargeUp");

        // Do not do damage if the player did not properly charge
        if (chargeMultiplier <= 1.25f)
        {
            ResetValues();
            return;
        }

        RaycastHit[] hits = Physics.RaycastAll(cam.position, camRotation.forward, raycastProjectileData.maxDistance, targetLayers);
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
                damageable.Damage(raycastProjectileData.damage * chargeMultiplier * GetAtkMultiplier());
                
                AudioManager.Instance.PlaySFX("SFXLaserImpact");
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

    protected override void OnPrimary()
    {
        AudioManager.Instance.PlaySFX("SFXLaserShoot");
    }

    protected override void OnSecondary()
    {
        animator.SetTrigger("Primary");
        AudioManager.Instance.PlaySFX("SFXLaserShoot");
    }

    protected override void OnAbility()
    {
    }
}