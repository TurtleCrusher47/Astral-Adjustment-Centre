using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGauntlet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private PlayerCombat player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("PlayerCollider"))
        {
            player.Damage(damage);
            Debug.Log("Player Hit");
        }
    }

}
