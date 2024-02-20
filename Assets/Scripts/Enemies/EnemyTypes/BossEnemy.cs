using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField]
    public EnemyAttackSOBase enemyAttackOverrideBase;
    [SerializeField]
    public EnemyAttackSOBase enemyAreaAttackBase;
    [SerializeField]
    public EnemyAttackSOBase enemyUltimateAttackBase;
    [SerializeField]
    public EnemyAttackIndicator indicator;
}
