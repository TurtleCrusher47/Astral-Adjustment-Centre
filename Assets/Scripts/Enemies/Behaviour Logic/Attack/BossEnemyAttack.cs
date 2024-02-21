using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack- Boss Attack", menuName = "Enemy Logic/Attack State/Boss Attack")]

public class BossEnemyAttack : EnemyAttackSOBase
{
    private Animator animator;
    private BossEnemy self;

    public Vector3 _attackRange;
    public float _posOffset;
    public float _chargeUpTimer;
    private float _timer;

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        //any get components, other entry logic do here
        animator = enemy.gameObject.GetComponent<Animator>();
        self = transform.GetComponent<BossEnemy>();
        // play back swing anim
        animator.SetTrigger("isPunch");
        _timer = _chargeUpTimer;
        // set attack indicator
        self.indicator.gameObject.SetActive(true);
        self.indicator.SetChargeParameters(
                                            transform.position + (transform.forward * _posOffset), 
                                            new Vector3(0, transform.localEulerAngles.y, 0),
                                            _attackRange
                                            );
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
        
        //exit logic here
        //if you need to reset any values, return anything to pool at end of state
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        //do state logic here
        enemy.MoveEnemy(Vector3.zero);
        // charge timer
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else if (_timer <= 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                // play follow through anim
                animator.SetBool("isPunchEnd", true);
            }
            // wait for follow through anim to finish
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                // swap back to chase
                enemy.stateMachine.ChangeState(enemy.chaseState);
                animator.SetBool("isPunchEnd", false);
                self.indicator.gameObject.SetActive(false);
            }
        }
        // update attack indicator
        self.indicator.DoCharge(_chargeUpTimer, _timer);
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

