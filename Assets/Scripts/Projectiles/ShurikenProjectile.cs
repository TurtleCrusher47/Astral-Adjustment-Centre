using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenProjectile : GameObjectProjectile
{
    protected override void SetAngularVelocity()
    {
        angularVelocity = camRotation.up;

        // Freeze rotation for X and Z axis at the start
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerCollider") || collider.gameObject.CompareTag("WeaponCollider") || collider.gameObject.CompareTag("AggroRadius") || collider.gameObject.CompareTag("StrikingDistance"))
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
            rb.constraints = RigidbodyConstraints.FreezeRotationY;
            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject, 3));
        }

        AudioManager.Instance.PlaySFX("SFXShurikenImpact");

        //Debug.Log(collider.gameObject.name);
    }
}
