using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Waypoint Patrol", menuName = "Enemy Logic/Idle Logic/Waypoint Patrol")]

public class EnemyIdleWaypointPatrol : EnemyIdleSOBase
{

    //change this to take in existing game objects? not sure if that is possible from so tho 
     [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>();

    [SerializeField] public float movementSpeed = 1f;
    private int _targetIndex = 0;
    private Vector3 _targetPos;
    private Vector3 _direction;
    private float _pauseTimer = 2f;
    private float _timer = 0f;
     public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        waypoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoint"));
        _targetPos = waypoints[_targetIndex].transform.position;
        _direction = ( _targetPos - transform.position).normalized;
        enemy.gameObject.transform.LookAt(_targetPos);

    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(_direction * movementSpeed);

        Debug.Log(Vector3.Distance(transform.position, _targetPos));

        if (Vector3.Distance(transform.position, _targetPos) <= 0.1f)
        {
            //add timer to pause at waypoint
            _timer += Time.deltaTime;

            if (_timer <= _pauseTimer)
            {
                enemy.MoveEnemy(Vector3.zero);
            }

            else
            {
                if (_targetIndex >= waypoints.Count)
                {
                    _targetIndex = 0;
                }
                else
                {
                    _targetIndex++;
                }

                _targetPos = waypoints[_targetIndex].transform.position;
                _direction = ( _targetPos - transform.position).normalized;
                enemy.gameObject.transform.LookAt(_targetPos);
                _timer = 0f;
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
