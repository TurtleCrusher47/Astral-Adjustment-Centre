using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack- Gas", menuName = "Enemy Logic/Attack State/Gas Attack")]

public class EnemyAttackGas : EnemyAttackSOBase
{
    [SerializeField] private float _gasCooldown = 3f;

    [SerializeField] private float _timeTillExit = 2f;
    [SerializeField] private float _distanceToCountExit = 3f;
    private Animator gasAnimator;

    private Animator animator;
    private GameObject gasObject;


    private float _exitTimer;
    private float _timer;
   
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        gasObject.SetActive(true);

        animator.SetTrigger("isLaugh");
        gasAnimator.SetTrigger("isExpand");
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();

        gasAnimator.ResetTrigger("isExpand");
        gasAnimator.SetTrigger("isShrink");
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector3.zero);

        Vector3 lookPos = (playerTransform.transform.position - transform.position).normalized;
        lookPos.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);

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
        gasAnimator = enemy.gameObject.GetComponent<GasEnemy>().gasAnimator;
        gasObject = enemy.gameObject.GetComponent<GasEnemy>().gasObject;

    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
