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
        if ( other.gameObject.CompareTag("WeaponCollider"))
        return;

        if (other.CompareTag("PlayerCollider") && !inInventory)
        {
            other.transform.parent.GetComponent<PlayerWeaponPickup>().PickUpWeapon(gameObject);
        }

        if (other.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(meleeWeaponData.damage * BuffManager.Instance.buffs[1].buffBonus[BuffManager.Instance.buffs[1].currBuffTier]);
            // StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
        }
    }

    protected override void UsePrimary()
    {
        int i = 0;
        attackQueue.Enqueue(i);

        Debug.Log("use sword primary");
        Debug.Log("Queue " + attackQueue.Count);
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
