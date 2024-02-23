using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack- Single Straight Shot", menuName = "Enemy Logic/Attack State/Single Straight Shot")]

public class EnemyAttackSingleStraightProjectile : EnemyAttackSOBase
{
    public GameObject BulletPrefab;
   
    [SerializeField] private float _shotCooldown = 3f;
    [SerializeField] private float _bulletSpeed = 1f;

    [SerializeField] private float _timeTillExit = 2f;
    [SerializeField] private float _distanceToCountExit = 4.5f;

    private RangedEnemy rangedEnemy;


    private float _exitTimer;
    private float _timer;
   
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        rangedEnemy = enemy.gameObject.GetComponent<RangedEnemy>();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        transform.GetComponent<RangedEnemy>().HideIndicator();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector3.zero);

        Vector3 lookPos = (playerTransform.transform.position - transform.position).normalized;
        lookPos.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);

        _timer += Time.deltaTime;

        if (_timer > _shotCooldown - 0.5f)
        {
            transform.GetComponent<RangedEnemy>().ShowIndicator();
        }
        if (_timer > _shotCooldown)
        {
            _timer = 0f;

            Vector3 dir = (playerTransform.position - rangedEnemy.firePoint.position).normalized;

            GameObject bullet = ObjectPoolManager.Instance.SpawnObject(BulletPrefab, rangedEnemy.firePoint.position, enemy.transform.localRotation);

            bullet.GetComponent<EnemyProjectileBasic>().ScaleProjectile(playerTransform.localScale);
            bullet.GetComponent<EnemyProjectileBasic>().MoveProjectile(dir * _bulletSpeed);

            transform.GetComponent<RangedEnemy>().HideIndicator();
        }

        if (Vector3.Distance(playerTransform.position, enemy.transform.position) > (_distanceToCountExit * playerTransform.localScale.x))
        {
            _exitTimer += Time.deltaTime;

            if (_exitTimer >= _timeTillExit)
            {
                enemy.stateMachine.ChangeState(enemy.chaseState);
            }
        }

        else
        {
            _exitTimer = 0;
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
