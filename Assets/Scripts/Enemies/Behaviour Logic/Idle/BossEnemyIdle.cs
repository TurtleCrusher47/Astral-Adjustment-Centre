using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Boss Idle", menuName = "Enemy Logic/Idle Logic/Boss Idle")]
public class BossEnemyIdle : EnemyIdleSOBase
{
    public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        //any get components, other entry logic do here
        //var component = gameObject.GetComponent<something>();
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

        //example for if you wanna move the enemy
        //Vector3 moveDir = some vector3;
        //enemy.MoveEnemy(moveDir);

        //if you need to reference this enemy's transform or gameObject they are defined in the base class
        //transform.localScale = Vector3(10, 10 ,10);
        //gameObject.GetComponent<WHATEVER U WANT>();

        // The players transform can be called, is also defined in base class
        //playerTransform.whateverThingUWantFromTranformBro = (idk man anything u want);

        //if u wanna change state 
        //this example is after a certain distance away and like cetain time has passed
        // if (Vector3.Distance(playerTransform.position, enemy.transform.position) > (_distanceToCountExit * playerTransform.localScale.x))
        // {
        //     _exitTimer += Time.deltaTime;

        //     if (_exitTimer >= _timeTillExit)
        //     {
        //         enemy.stateMachine.ChangeState(enemy.chaseState);
        //     }
        // }

        // else
        // {
        //     _exitTimer = 0;
        // }

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
