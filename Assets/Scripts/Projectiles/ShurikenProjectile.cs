using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenProjectile : GameObjectProjectile
{
    public override void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        return;

        Debug.Log(collider.transform.name);
        IDamageable damageable = collider.transform.GetComponent<IDamageable>();
        damageable?.Damage(gameObjectProjectileData.damage);

        // Return back to object pool
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}
