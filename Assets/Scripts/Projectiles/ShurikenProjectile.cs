using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenProjectile : GameObjectProjectile
{
    protected override void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = gameObjectProjectileData.angularVelocity;
        rb.constraints = ~RigidbodyConstraints.FreezePosition;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerCollider") || collider.gameObject.CompareTag("WeaponCollider"))
        return;

        // Debug.Log(collider.transform.name);
        // IDamageable damageable = collider.transform.GetComponent<IDamageable>();
        // damageable?.Damage(gameObjectProjectileData.damage);
        if (collider.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(gameObjectProjectileData.damage * BuffManager.Instance.buffs[1].buffBonus[BuffManager.Instance.buffs[1].currBuffTier]);
            // StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject, 3));
        }

        // Debug.Log(collider.name);
    }
}
