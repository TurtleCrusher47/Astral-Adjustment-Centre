using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] public float damageFrequency = 1.5f;
    private PlayerCombat player;
    private float damageTimer = 0;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PlayerCollider"))
        {
            damageTimer = 0;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("PlayerCollider"))
        {
            damageTimer = 0;
        }
    }
    
    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("PlayerCollider"))
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageFrequency)
            {
                player.Damage(damage);
                damageTimer = 0;
            }
        }
    }

    public void DeactivateGas()
    {
        gameObject.SetActive(false);
    }
}
