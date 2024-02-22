using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : GameObjectRangedWeapon
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float lazerRange;
    private float elapsedTime = 0;

    protected override void UseSecondary()
    {
        // If player starts reloading while charging
        if (rangedWeaponData.reloading)
        {
            elapsedTime = 0;
        }
        else if (rangedWeaponData.currentAmmo - 1 >= 0)
        {
            if (Physics.Raycast(cam.position, camRotation.forward, out RaycastHit hitInfo, lazerRange, targetLayers))
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= 1f)
                {
                    elapsedTime %= 1f;
                    rangedWeaponData.currentAmmo -= 1;

                    Debug.Log(hitInfo.transform.name);
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gameObjectProjectileData.damage * GetAtkMultiplier());
                    
                    // GameObject effect = ObjectPoolManager.SpawnObject(hitEffect, hitInfo.point, hitInfo.transform.rotation);
                    // Destroy(effect, 0.5f);
                }
                RenderLine(hitInfo.point);
                animator.SetBool("isSecondary", true);
            }
        }
    }

    protected override void UseSecondaryUp()
    {
        lineRenderer.positionCount = 0;
        animator.SetBool("isSecondary", false);
    }

    protected override void OnSecondary()
    {
        
    }

    private void RenderLine(Vector3 hitPosition)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, hitPosition);

        Debug.Log("Rendering Line");
    }

    protected override void OnAbility()
    {
    }


    protected override void UseAbility()
    {
    }
}
