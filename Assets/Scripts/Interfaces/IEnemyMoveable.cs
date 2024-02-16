using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody rb { get; set; }
    void MoveEnemy(Vector3 velocity);

}
