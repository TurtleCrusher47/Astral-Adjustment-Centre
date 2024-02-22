using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProjectile : GameObjectProjectile
{
    [SerializeField] float explosiveTimer = 3;
    private bool isTriggered = false;

    protected override void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = gameObjectProjectileData.angularVelocity;
        isTriggered = false;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerCollider") || collider.gameObject.CompareTag("WeaponCollider"))
        return;

        if (collider.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Explode();
        }
    }

    private void Explode()
    {
        // Check if it has already exploded
        if (!isTriggered)
        {
            foreach (var collider in Physics.OverlapSphere(transform.position, 3))
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(gameObjectProjectileData.damage);
                }
            }

            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
        }
    }

    private IEnumerator StartExplosiveTimer()
    {
        yield return new WaitForSeconds(explosiveTimer);

        Explode();
    }
}
