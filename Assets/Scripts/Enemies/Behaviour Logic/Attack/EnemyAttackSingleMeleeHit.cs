using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack- Single Melee Hit", menuName = "Enemy Logic/Attack State/Single Melee Hit")]

public class EnemyAttackSingleMeleeHit : EnemyAttackSOBase
{
    [SerializeField] private float _hitCooldown = 2f;

    [SerializeField] private float _timeTillExit = 2f;
    [SerializeField] private float _distanceToCountExit = 4.5f;

    private Animator animator;


    private float _exitTimer;
    private float _timer;
   
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
        animator = enemy.gameObject.GetComponent<Animator>();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
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

        if (_timer > _hitCooldown)
        {
            _timer = 0f;

            //trigger animator to play punch animation
            //set gauntlet trigger collider on and off in animation
            animator.SetTrigger("isPunch");
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

        animator = enemy.gameObject.GetComponent<Animator>();
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}

