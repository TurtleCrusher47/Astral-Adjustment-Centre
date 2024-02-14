using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody2D rb { get; set; }

    public bool isFacingRight { get; set; } = false;

#region state machine variables
      
      public EnemyStateMachine stateMachine { get; set; }
      public EnemyIdleState idleState { get; set; }
      public EnemyChaseState chaseState { get; set; }
      public EnemyAttackState attackState { get; set; }

    public bool isAggroed { get; set; }
    public bool isInStrikingDistance { get; set; }

#endregion
    
#region scriptable object variables

    [SerializeField]
    EnemyIdleSOBase enemyIdleBase;

    [SerializeField]
    EnemyChaseSOBase enemyChaseBase;

    [SerializeField]
    EnemyAttackSOBase enemyAttackBase;

    public EnemyIdleSOBase enemyIdleBaseInstance { get; set; }
    public EnemyChaseSOBase enemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }

#endregion

    void Awake()
    {
        
        stateMachine = new EnemyStateMachine();

        idleState = new EnemyIdleState(this, stateMachine);

        chaseState = new EnemyChaseState(this, stateMachine);

        attackState = new EnemyAttackState(this, stateMachine);

        enemyIdleBaseInstance = Instantiate(enemyIdleBase);
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);

        CurrentHealth = MaxHealth;

        rb = GetComponent<Rigidbody2D>();

        enemyIdleBaseInstance.Init(gameObject, this);
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);

        stateMachine.Init(idleState);   
        
    }

    private void Update()
    {
        stateMachine.currEnemyState.FrameUpdate();

    }

    private void FixedUpdate()
    {
        stateMachine.currEnemyState.PhysicsUpdate();
    }


#region health functions
    public void Damage(int damage)
    {
       CurrentHealth -= damage;

       if (CurrentHealth <= 0)
       {
            Despawn();
       }
    }

    public void Despawn()
    {
        //change later to use object pooling
        Destroy(gameObject);
    }

#endregion

#region movement functions

    public void MoveEnemy(Vector2 velocity)
    {
        rb.velocity = velocity;
        CheckDirectionFacing(velocity);

    }

    public void CheckDirectionFacing(Vector2 velocity)
    {
       if (isFacingRight && velocity.x < 0f)
       {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
       }

        else if (isFacingRight && velocity.x > 0f)
       {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
       }
    }

#endregion

#region animation triggers
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        stateMachine.currEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
        {
            EnemyDamaged,
            PlayFootstepSound
        }
#endregion

#region trigger checks

     public void SetAggroStatus(bool isAggroed)
    {
        this.isAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isInStrikingDistance)
    {
       this.isInStrikingDistance = isInStrikingDistance;
    }

#endregion
}
