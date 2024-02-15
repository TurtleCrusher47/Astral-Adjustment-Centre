using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectProjectile : MonoBehaviour
{
   [SerializeField] public GameObjectProjectileData gameObjectProjectileData;

    protected Rigidbody rb;
    private Transform cam;
    private Transform firePoint;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        cam = GameObject.FindWithTag("MainCamera").transform;
        firePoint = GameObject.FindWithTag("FirePoint").transform;
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = gameObjectProjectileData.angularVelocity;

        // Call a coroutine to return bullet to pool after a set amount of time
        Vector3 shootDirection = cam.forward;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, gameObjectProjectileData.range))
        {
            shootDirection = (hit.point - firePoint.position).normalized;
            // Debug.Log(cam.forward);
        }

        Vector3 forceToAdd = shootDirection * gameObjectProjectileData.force;

        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    public abstract void OnTriggerEnter(Collider collider);
}
