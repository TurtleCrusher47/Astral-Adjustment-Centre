using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState currEnemyState { get; set; }

    public void Init(EnemyState startingState)
    {
        currEnemyState = startingState;
        currEnemyState.EnterState();
    }

    public void ChangeState(EnemyState nextState)
    {
        currEnemyState.ExitState();
        currEnemyState = nextState;
        currEnemyState.EnterState();
    }
}
