using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveKunaiProjectile : GameObjectProjectile
{
    [SerializeField] private GameObject effectObject;

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
