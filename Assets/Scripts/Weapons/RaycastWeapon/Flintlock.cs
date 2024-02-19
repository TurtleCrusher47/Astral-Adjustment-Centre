using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flintlock : RaycastRangedWeapon
{
    [SerializeField] private GameObject abilityBarrel;
    [SerializeField] private LayerMask groundMask;

    protected override void OnSecondary()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSkill()
    {
        if (CanUseAbility())
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, 3, groundMask, QueryTriggerInteraction.Collide))
            {
                GameObject barrel = ObjectPoolManager.Instance.SpawnObject(abilityBarrel, hitInfo.transform.position, orientation.rotation, ObjectPoolManager.PoolType.Projectile);
            }
        }
    }

    protected override void UseAbility()
    {
        throw new System.NotImplementedException();
    }

    protected override void UseSecondary()
    {
        throw new System.NotImplementedException();
    }
}
