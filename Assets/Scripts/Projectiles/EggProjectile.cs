using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProjectile : GameObjectProjectile
{
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
    }
}
