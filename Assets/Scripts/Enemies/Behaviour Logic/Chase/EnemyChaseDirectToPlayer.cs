using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chase- Direct Chase", menuName = "Enemy Logic/Chase State/Direct Chase")]
public class EnemyChaseDirectToPlayer : EnemyChaseSOBase
{
    [SerializeField] private float _movementSpeed = 4.0f;

    private Animator animator;
   
   public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        animator = gameObject.GetComponent<Animator>();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        Vector3 moveDirection = (playerTransform.position - enemy.transform.position).normalized;
        moveDirection.y = 0;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("YBotPunch"))
        {
            enemy.MoveEnemy(Vector3.zero);
        }

        else
        {
            enemy.MoveEnemy(moveDirection * _movementSpeed * playerTransform.localScale.x);
        }


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
