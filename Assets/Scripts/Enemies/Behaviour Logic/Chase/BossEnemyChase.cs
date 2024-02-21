using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase- Boss Chase", menuName = "Enemy Logic/Chase State/Boss Chase")]
public class BossEnemyChase : EnemyChaseSOBase
{
    [SerializeField] private float _movementSpeed = 4.0f;

    [SerializeField] private float _attackCooldown;
    private float _timer;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        _timer = _attackCooldown;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        BossEnemy self = transform.GetComponent<BossEnemy>();
        if (enemy.isInStrikingDistance && _timer <= 0)
        {
            self.attackState = new EnemyAttackState(self, self.stateMachine);
            if (self.CurrentHealth <= self.MaxHealth / 2)
            {
                int check = Random.Range(0, self.enemyAttackOverrideBase.Count + 2);
                if (check >= self.enemyAttackOverrideBase.Count)
                {
                    self.enemyAttackBaseInstance = Instantiate(self.enemyUltimateAttackBase);
                    self.enemyAttackBaseInstance.Init(gameObject, self);
                }
                else
                {
                    self.enemyAttackBaseInstance = Instantiate(self.enemyAttackOverrideBase[check]);
                    self.enemyAttackBaseInstance.Init(gameObject, self);
                }
            }
            else
            {
                int check = Random.Range(0, self.enemyAttackOverrideBase.Count);
                self.enemyAttackBaseInstance = Instantiate(self.enemyAttackOverrideBase[check]);
                self.enemyAttackBaseInstance.Init(gameObject, self);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1);

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
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
