using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    protected override void Start()
    {
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCollider") || other.gameObject.CompareTag("WeaponCollider"))
        return;

        if (other.CompareTag("PlayerCollider") && !inInventory)
        {
            other.transform.parent.GetComponent<PlayerWeaponPickup>().PickUpWeapon(gameObject);
        }

        if (other.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(meleeWeaponData.damage);
            // StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
        }
    }

    protected override void UsePrimary()
    {
        attackQueue.Enqueue(null);
    }

    protected override void UseAbility()
    {
        throw new System.NotImplementedException();
    }

    protected override void UseSecondary()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnPrimary()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnAbility()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSecondary()
    {
        throw new System.NotImplementedException();
    }
}
