using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProjectile : GameObjectProjectile
{
    [SerializeField] float explosiveTimer = 3;
    [SerializeField] private GameObject effectObject;
    private bool isTriggered = false;

    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("PlayerCollider") || collider.gameObject.CompareTag("WeaponCollider"))
        return;

        if (collider.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Explode();
        }
        else
        {
            StartCoroutine(StartExplosiveTimer());
        }
    }

    private void Explode()
    {
        // Check if it has already exploded
        if (!isTriggered)
        {
            foreach (var collider in Physics.OverlapSphere(transform.position, 3 * 0.15f))
            {
                if (collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.Damage(gameObjectProjectileData.damage * GetAtkMultiplier());
                }
            }

            GameObject explosion = ObjectPoolManager.Instance.SpawnObject(effectObject, transform.position, Quaternion.identity);
            explosion.GetComponent<Transform>().localScale = new Vector3(0.15f, 0.15f, 0.15f);
            AudioManager.Instance.PlaySFX("SFXExplosion");
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(explosion, 1.9f));

            StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
        }
    }

    private IEnumerator StartExplosiveTimer()
    {
        yield return new WaitForSeconds(explosiveTimer);

        Explode();
    }
}
