using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField]
    public List<EnemyAttackSOBase> enemyAttackOverrideBase;
    [SerializeField]
    public EnemyAttackSOBase enemyUltimateAttackBase;
    [SerializeField]
    public EnemyAttackIndicator indicator;
}
