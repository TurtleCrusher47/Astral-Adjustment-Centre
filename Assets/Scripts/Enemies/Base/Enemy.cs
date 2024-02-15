using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public Rigidbody rb { get; set; }

    //public bool isFacingRight { get; set; } = false;

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

        rb = GetComponent<Rigidbody>();

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

    public void MoveEnemy(Vector3 velocity)
    {
        rb.velocity = velocity;
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

    public List<GameObject> FindChildObjectsWithTag(GameObject parent, string tag)
    {
        List<GameObject> children = new();
 
        foreach(Transform transform in parent.transform) 
        {
            if(transform.CompareTag(tag)) 
            {
                children.Add(transform.gameObject);
            }
        }
        
        return children;
    }
}
