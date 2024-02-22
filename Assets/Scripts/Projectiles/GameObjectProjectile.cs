using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectProjectile : MonoBehaviour
{
   [SerializeField] public GameObjectProjectileData gameObjectProjectileData;

    protected Rigidbody rb;
    private Transform cam;
    private Transform firePoint;
    [HideInInspector] public Vector3 projectileDirection;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        cam = GameObject.FindWithTag("CameraHolder").transform;
        firePoint = GameObject.FindWithTag("FirePoint").transform;
    }

    protected abstract void OnEnable();

    public void MoveProjectile()
    {
        // Call a coroutine to return bullet to pool after a set amount of time
        Vector3 shootDirection = projectileDirection;

        // RaycastHit hit;
        // if (Physics.Raycast(cam.position, cam.forward, out hit, gameObjectProjectileData.range))
        // {
        //     shootDirection = (hit.point - firePoint.position).normalized;
        //     // Debug.Log(cam.forward);
        // }

        Vector3 forceToAdd = shootDirection * gameObjectProjectileData.force;

        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    public abstract void OnTriggerEnter(Collider collider);
}
