using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = transform.parent.gameObject.GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerCollider"))
        {
            _enemy.SetAggroStatus(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
         if (collision.CompareTag("PlayerCollider"))
        {
            _enemy.SetAggroStatus(false);
        }
    }
}
