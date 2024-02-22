using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveKunaiProjectile : GameObjectProjectile
{
    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerCollider") || collider.gameObject.CompareTag("WeaponCollider") || collider.gameObject.CompareTag("AggroRadius") || collider.gameObject.CompareTag("StrikingDistance"))
        return;

        // Debug.Log(collider.transform.name);
        // IDamageable damageable = collider.transform.GetComponent<IDamageable>();
        // damageable?.Damage(gameObjectProjectileData.damage);
        rb.constraints = RigidbodyConstraints.FreezePosition;
        this.transform.SetParent(collider.transform);
        StartCoroutine(Explode());
    }

    protected override void SetAngularVelocity()
    {
        throw new System.NotImplementedException();
    }

    // Wait for a few seconds then explode
    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);

        foreach (var collider in Physics.OverlapSphere(transform.position, 3))
        {
            if (collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(gameObjectProjectileData.damage * GetAtkMultiplier());

                AudioManager.Instance.PlaySFX("SFXExplosion");
            }
        }
        
        StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
    }
}
