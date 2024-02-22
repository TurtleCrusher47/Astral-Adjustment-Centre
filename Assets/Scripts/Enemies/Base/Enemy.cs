using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth { get; set; } = 100f;
    [field: SerializeField] public float CurrentHealth { get; set; }

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider easeHealthSlider;

    [SerializeField] private float lerpSpeed = 0.05f; 

    public Rigidbody rb { get; set; }
    public Animator animator;
    private Generator3D generator;
    private bool isDead = false;
    private float immunityTimer = 0;

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

    void OnEnable()
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
        animator = GetComponent<Animator>();

        enemyIdleBaseInstance.Init(gameObject, this);
        enemyChaseBaseInstance.Init(gameObject, this);
        enemyAttackBaseInstance.Init(gameObject, this);

        stateMachine.Init(idleState);   
        
        generator = GameObject.FindGameObjectWithTag("TradeButton").GetComponent<Generator3D>();

        immunityTimer = 0;
        isDead = false;
    }

    private void Update()
    {
        stateMachine.currEnemyState.FrameUpdate();
        UpdateAnimator();
        HealthBarSlider();

        if (immunityTimer > 0 && !isDead)
        {
            immunityTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        stateMachine.currEnemyState.PhysicsUpdate();
    }

    private void UpdateAnimator()
    {
        if (isDead)
        return;

        if (Mathf.Abs(rb.velocity.x) > 0.01f || Mathf.Abs(rb.velocity.z) > 0.01f)
        {
            animator.SetBool("isWalking", true);
        }

        else
        {
            animator.SetBool("isWalking", false);
        }
    }


#region health functions
    public void Damage(float damage)
    {
        if (isDead)
        return;

        if (immunityTimer <= 0)
        {
            CurrentHealth -= damage;

            CurrentHealth = Mathf.Max(CurrentHealth, 0f);

            animator.SetTrigger("isHit");

            if (CurrentHealth <= 0)
            {
                animator.SetTrigger("isDead");
                isDead = true;
                rb.isKinematic = true;
            }
            immunityTimer = 0.5f;
        }
    }

    public void Despawn()
    {
        //change later to use object pooling
        rb.isKinematic = false;
        generator.RemoveEnemyFromRoom(gameObject.transform.parent.gameObject);
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

#region Enemy Health Bar

    public void HealthBarSlider()
    {
        if(healthSlider.value != CurrentHealth)
        {
            healthSlider.value = CurrentHealth;
        }

        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, CurrentHealth, lerpSpeed);
        }

        //Debug.Log("Enemy Health: " + healthSlider.value);
    }

#endregion
}
