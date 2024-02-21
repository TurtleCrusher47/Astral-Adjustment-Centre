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


    private float _exitTimer;
    private float _timer;
   
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
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
