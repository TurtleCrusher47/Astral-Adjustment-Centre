using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase- Boss Chase", menuName = "Enemy Logic/Chase State/Boss Chase")]
public class BossEnemyChase : EnemyChaseSOBase
{
    [SerializeField] private float _movementSpeed = 4.0f;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        BossEnemy self = transform.GetComponent<BossEnemy>();
        if (enemy.isInStrikingDistance)
        {
            self.attackState = new EnemyAttackState(self, self.stateMachine);
            if (self.CurrentHealth <= self.MaxHealth / 2)
            {
                Debug.Log("Ultimate");
                self.enemyAttackBaseInstance = Instantiate(self.enemyUltimateAttackBase);
                self.enemyAttackBaseInstance.Init(gameObject, self);
            }
            else
            {
                int check = Random.Range(0, 2);
                switch (check)
                {
                case 0:
                        Debug.Log("Normal");
                        self.enemyAttackBaseInstance = Instantiate(self.enemyAttackOverrideBase);
                        self.enemyAttackBaseInstance.Init(gameObject, self);
                        break;
                case 1:
                        Debug.Log("AOE");
                        self.enemyAttackBaseInstance = Instantiate(self.enemyAreaAttackBase);
                        self.enemyAttackBaseInstance.Init(gameObject, self);
                        break;
                }
            }
            enemy.stateMachine.ChangeState(enemy.attackState);
        }
        if (!enemy.isAggroed)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }

        Vector3 moveDirection = (playerTransform.position - enemy.transform.position).normalized;
        moveDirection.y = 0;

        enemy.MoveEnemy(moveDirection * _movementSpeed);

        Vector3 lookPos = (playerTransform.transform.position - transform.position).normalized;
        lookPos.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);
    }

    public override void DoPhysicsLogic()
    {
        base.DoPhysicsLogic();
    }

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
