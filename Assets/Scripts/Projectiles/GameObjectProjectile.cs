using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObjectProjectile : MonoBehaviour
{
   [SerializeField] public GameObjectProjectileData gameObjectProjectileData;
   [SerializeField] private float angularVelocityMultiplier;
   protected Vector3 angularVelocity;

    protected Rigidbody rb;
    private Transform cam;
    protected Transform camRotation;
    private Transform firePoint;
    [HideInInspector] public Vector3 projectileDirection;

    protected ScriptableBuff atkBuff;
    protected float atkBuffMultiplier;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        camRotation = GameObject.FindGameObjectWithTag("CameraRotation").transform;
        firePoint = GameObject.FindGameObjectWithTag("FirePoint").transform;
        angularVelocity = camRotation.right;
    }

    // Unable to put abstract or virtual OnAwake as Unity does not support it :)
    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.constraints &= ~RigidbodyConstraints.FreezePosition;
        // Debug.Log("Enabled");

        SetAngularVelocity();
    }

    protected virtual void SetAngularVelocity()
    {
        angularVelocity = Vector3.zero;
    }

    protected float GetAtkMultiplier()
    {
        atkBuff = BuffManager.Instance.buffs[0];
        if (atkBuff.currBuffTier > 0)
        {
            atkBuffMultiplier = atkBuff.buffBonus[atkBuff.currBuffTier - 1];
        }
        else
        {
            atkBuffMultiplier = 1;
        }
        return atkBuffMultiplier;
    }

    public void MoveProjectile()
    {
        // Call a coroutine to return bullet to pool after a set amount of time
        Vector3 shootDirection = projectileDirection;
        rb.angularVelocity = angularVelocity * angularVelocityMultiplier;
        Debug.Log(rb.angularVelocity);

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
