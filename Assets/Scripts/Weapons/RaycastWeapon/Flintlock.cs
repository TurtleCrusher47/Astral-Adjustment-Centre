using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flintlock : RaycastRangedWeapon
{
    [SerializeField] private GameObject abilityBarrel;
    [SerializeField] protected LayerMask secondaryTargetLayers;
    [SerializeField] private LayerMask groundMask;

    protected override void OnSecondary()
    {
        
    }

    protected override void OnSkill()
    {
        
    }

    protected override void UseAbility()
    {
        if (CanUseAbility())
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, 1, groundMask))
            {
                GameObject barrel = ObjectPoolManager.Instance.SpawnObject(abilityBarrel, new Vector3(hitInfo.point.x, cam.transform.position.y, hitInfo.point.z), abilityBarrel.transform.rotation, ObjectPoolManager.PoolType.Projectile);
                Debug.Log("Raycast Hit");
            }
            else
            {
                GameObject barrel = ObjectPoolManager.Instance.SpawnObject(abilityBarrel, cam.transform.position, abilityBarrel.transform.rotation, ObjectPoolManager.PoolType.Projectile);
            }
            abilityCooldownTimer = rangedWeaponData.abilitycooldown;
        }
    }

    protected override void UseSecondary()
    {
        if (CanUseSecondary())
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, raycastProjectileData.maxDistance, secondaryTargetLayers, QueryTriggerInteraction.Collide))
            {
                StartCoroutine(RenderTraceLine(hitInfo.point));
                Debug.Log(hitInfo.transform.name);
                IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                damageable?.Damage(raycastProjectileData.damage);
            }
            else
            {
                StartCoroutine(RenderTraceLine(cam.forward * raycastProjectileData.maxDistance));
            }

            
            secondaryCooldownTimer = rangedWeaponData.secondarycooldown;
            OnSecondary();
        }
        
    }
}
