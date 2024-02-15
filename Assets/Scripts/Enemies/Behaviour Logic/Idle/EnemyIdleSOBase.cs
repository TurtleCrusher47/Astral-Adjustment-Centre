using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;

    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
        this.enemy = enemy;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Debug.Log("Init run");
    }

    public virtual void DoEnterLogic(){}
    public virtual void DoExitLogic()
    {
        ResetValues();
    }
    public virtual void DoFrameUpdateLogic()
    {
         if (enemy.isAggroed)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
        }

    }
    public virtual void DoPhysicsLogic(){}
    public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType){}
    public virtual void ResetValues(){}
    
}
