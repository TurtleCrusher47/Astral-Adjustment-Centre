using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    // [SerializeField] Animator animator;
    private float health = 100;
    private float immunityTimer = 0;
    private bool isDead;

    private void Update()
    {
        if (immunityTimer > 0 && !isDead)
        {
            immunityTimer -= Time.deltaTime;
        }
    }

    public void Damage(float damage)
    {
        if (immunityTimer <= 0)
        {
            health -= damage;
            Debug.Log("Hit");
            // animator.SetTrigger("hit");

            if (health <= 0)
            {
                isDead = true;
                // animator.SetBool("isDead", isDead);

                Despawn();
            }
            immunityTimer = 0.5f;

            // Debug.Log(health);
        }
    }

    public void Despawn()
    {
        // Return to pool
        StartCoroutine(ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject));
    }
}
