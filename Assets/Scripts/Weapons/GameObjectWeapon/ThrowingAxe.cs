using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingAxe : GameObjectRangedWeapon
{
    [SerializeField] Transform firePoint2;

    protected override void OnAbility()
    {
    }

    protected override void OnSecondary()
    {
        animator.SetTrigger("Secondary");
        AudioManager.Instance.PlaySFX("SFXShurikenThrow");
    }

    protected override void OnPrimary()
    {
        animator.SetTrigger("Primary");
        AudioManager.Instance.PlaySFX("SFXShurikenThrow");
    }

    protected override void UseAbility()
    {
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {
            // Debug.Log("Shot GameObject");
            GameObject currentProjectile = ObjectPoolManager.Instance.SpawnObject(projectile, firePoint2.position, camRotation.rotation, ObjectPoolManager.PoolType.Projectile);
            currentProjectile.GetComponent<GameObjectProjectile>().projectileDirection = camRotation.forward;

            SetDirectionSecondary(currentProjectile);

            currentProjectile.GetComponent<GameObjectProjectile>().MoveProjectile();

            secondaryCooldownTimer = rangedWeaponData.secondarycooldown;
            OnSecondary();
        }
    }

    protected void SetDirectionSecondary(GameObject projectile)
    {
        RaycastHit[] hits = Physics.RaycastAll(cam.position, camRotation.forward, gameObjectProjectileData.range, targetLayers);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.layer == targetLayers)
            projectile.GetComponent<GameObjectProjectile>().projectileDirection = (hits[i].point - firePoint2.position).normalized;
        }
    }
}
