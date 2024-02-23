using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{

    public override void StartFunctionality()
    {

    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        if ( other.gameObject.CompareTag("WeaponCollider")|| GetComponent<Collider>().gameObject.CompareTag("Player") || GetComponent<Collider>().gameObject.CompareTag("AggroRadius") || GetComponent<Collider>().gameObject.CompareTag("StrikingDistance"))
        return;

        if (other.CompareTag("PlayerCollider") && !inInventory)
        {
            other.transform.parent.GetComponent<PlayerWeaponPickup>().PickUpWeapon(gameObject);
        }

        if (other.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(meleeWeaponData.damage * GetAtkMultiplier());
            AudioManager.Instance.PlaySFX("SFXMeleeImpact");
            // StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
        }
    }

    protected override void UsePrimary()
    {
        int i = 0;
        attackQueue.Enqueue(i);
    }

    protected override void UseAbility()
    {
        return;
    }

    protected override void UseSecondary()
    {
        return;
    }

    protected override void OnPrimary()
    {
        return;
    }

    protected override void OnAbility()
    {
        return;
    }

    protected override void OnSecondary()
    {
        return;
    }
}
