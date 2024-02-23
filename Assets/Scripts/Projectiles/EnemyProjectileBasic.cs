using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBasic : MonoBehaviour
{
    [SerializeField] public int damage;
    private PlayerCombat player;
    private Rigidbody rb;
    private Transform transform;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
        rb = gameObject.GetComponent<Rigidbody>();
        transform = gameObject.transform;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("WeaponCollider") || collider.gameObject.CompareTag("AggroRadius") || collider.gameObject.CompareTag("StrikingDistance") || collider.gameObject.CompareTag("Enemy"))
        return;
        
        if (collider.CompareTag("PlayerCollider"))
        {
            player.Damage(damage);
        }

        StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(gameObject, 0));
        
    }

    public void MoveProjectile(Vector3 moveVector)
    {
        rb.velocity = moveVector;
    }

    public void ScaleProjectile(Vector3 scaleVector)
    {
        transform.localScale = scaleVector;
    }
}
