using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle-Waypoint Patrol", menuName = "Enemy Logic/Idle Logic/Waypoint Patrol")]

public class EnemyIdleWaypointPatrol : EnemyIdleSOBase
{   
    [SerializeField]
    private List<GameObject> waypoints = new List<GameObject>();

    [SerializeField] public float movementSpeed = 2.0f;
    private int _targetIndex = 0;
    private Vector3 _targetPos;
    private Vector3 _direction;
    private float _pauseTimer = 2f;
    private float _timer = 0f;
     public override void DoEnterLogic()
    {
        base.DoEnterLogic();

        waypoints = enemy.FindChildObjectsWithTag(enemy.gameObject.transform.parent.gameObject, "Waypoint");
        //Debug.Log(enemy.gameObject.transform.parent.gameObject.name);
        _targetPos = waypoints[_targetIndex].transform.position;
        _direction = ( _targetPos - transform.position).normalized;
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(_direction * movementSpeed * playerTransform.localScale.x);

        Vector3 lookPos = (waypoints[_targetIndex].transform.position - transform.position).normalized;
        lookPos.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        if (transform.rotation != lookRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3);
        }

        Debug.Log(Vector3.Distance(transform.position, _targetPos));
        if (Vector3.Distance(transform.position, _targetPos) <= 0.13f)
        {
            //add timer to pause at waypoint
            _timer += Time.deltaTime;

            if (_timer <= _pauseTimer)
            {
                enemy.MoveEnemy(Vector3.zero);

                int targetIndex = _targetIndex + 1;

                if (targetIndex >= waypoints.Count)
                {
                    targetIndex = 0;
                }
                
            }

            else
            {
                if (_targetIndex == waypoints.Count - 1)
                {
                    _targetIndex = 0;
                }
                else
                {
                    _targetIndex++;
                }

                _targetPos = waypoints[_targetIndex].transform.position;
                _direction = ( _targetPos - transform.position).normalized;

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
