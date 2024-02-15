using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack- Single Straight Shot", menuName = "Enemy Logic/Attack State/Single Straight Shot")]

public class EnemyAttackSingleStraightProjectile : EnemyAttackSOBase
{
    public Rigidbody BulletPrefab;
    private float _timer;
    private float _shotCooldown = 3f;

    private float _exitTimer;
    private float _timeTillExit = 3f;
    private float _distanceToCountExit = 0.25f;

   private float _bulletSpeed = 5f;
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
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector3.zero);

        _timer += Time.deltaTime;

        if (_timer > _shotCooldown)
        {
            _timer = 0f;

            Vector3 dir = (playerTransform.position - enemy.transform.position).normalized;

            Rigidbody bullet = GameObject.Instantiate(BulletPrefab, enemy.transform.position, enemy.transform.localRotation);

            bullet.velocity = dir * _bulletSpeed;

        }

        if (Vector3.Distance(playerTransform.position, enemy.transform.position) > _distanceToCountExit)
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer > _timeTillExit)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }

            else
            {
                _exitTimer = 0f;
            }
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
