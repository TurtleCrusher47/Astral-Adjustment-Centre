using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenProjectile : GameObjectProjectile
{
    protected override void SetAngularVelocity()
    {
        angularVelocity = camRotation.up;
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
            damageable.Damage(gameObjectProjectileData.damage * GetAtkMultiplier());
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
