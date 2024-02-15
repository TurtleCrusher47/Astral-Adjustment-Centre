using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D rb { get; set; }
    bool isFacingRight { get; set; }
    void MoveEnemy(Vector2 velocity);
    void CheckDirectionFacing(Vector2 velocity);

}
