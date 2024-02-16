using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    void Start()
    {
        MaxHealth = 50;
        CurrentHealth = MaxHealth;
    }
}
