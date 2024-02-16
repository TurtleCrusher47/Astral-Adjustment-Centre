using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
      private GameObject PlayerTarget { get; set; }
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerCollider"))
        {
            _enemy.SetStrikingDistanceBool(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
         if (collision.CompareTag("PlayerCollider"))
        {
            _enemy.SetStrikingDistanceBool(false);
        }
    }
}
