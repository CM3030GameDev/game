using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Act2Miniboss : MonoBehaviour
{
    private enum BossState
    {
        IDLE,
        NORMAL,
        AGITATED,
        ATTACK,
        BEAM,
        ULTIMATE,
        DEAD
    }

    private enum FacingDirection
    {
        FRONT,
        BACK,
        LEFT,
        RIGHT
    }

    [SerializeField] private BossState currentState = BossState.NORMAL;
    [SerializeField] private FacingDirection currentDirection = FacingDirection.FRONT;

    public UnityEvent bossDeath;

    [Header("Minion Passive")]
    [SerializeField] private float maxPassiveCooldown = 5f;
    private float currentPassiveCooldown;
    private bool isPassiveOn = false;

    [Header("Timers")]
    [SerializeField] private float maxNormalStateTimer = 5f;
    private float currentNormalStateTimer = 0f;
    [SerializeField] private float maxAgitatedStateTimer = 5f;
    private float currentAgitatedStateTimer = 0f;
    [SerializeField] private float maxIdleDuration = 2f;
    private float currentIdleDuration = 0f;
    private BossState stateAfterIdle;

    private bool isSecondNormal = false;
    private bool hasLandedHit = false;
    
    private bool hasBeamed = false;
    private Vector2 playerPos;
    private bool hasPlayerPos = false;


    [Header("Movement")]
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float agitatedMoveSpeed = 8f;
    private Rigidbody2D rb;
    //[SerializeField] private float stopDistance = 1f;
    private float moveSpeed = 0f;
    private Transform playerTransform;

    private MinibossBeamAttack beamAttack;
    private MinibossUltimate ultAttack;
    private MinibossPassive passive;
    private Vector3 currentMoveDir = Vector3.zero;
    private bool shouldMove = false;

    [SerializeField] private Animator animator;

    private bool hasDeathAnimPlayed = false;

    [SerializeField] private int maxHP = 10;

    //remove later, for testing
    [SerializeField] private int currentHP = 10;

    [SerializeField] private float maxCollisionCD = 1f;
    private float currentCollisionCD;
    private bool hasCollided = false;
    [SerializeField] private int collisionDamage = 3;

    [SerializeField] private MinibossAttackRange attackRange;
    [SerializeField] private int attackDamage = 5;

    private bool isInRange = false;
    private bool hasUltStarted = false;

    private bool hasStartedBeam = false;
    private BossState stateBeforeAttack = BossState.NORMAL;
    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCollisionCD = maxCollisionCD;
        currentAgitatedStateTimer = maxAgitatedStateTimer;
        currentNormalStateTimer = maxNormalStateTimer;
        currentIdleDuration = maxIdleDuration;
        currentPassiveCooldown = maxPassiveCooldown;
        currentState = BossState.NORMAL;
        isPassiveOn = true;
        beamAttack = GetComponent<MinibossBeamAttack>();
        ultAttack = GetComponent<MinibossUltimate>();
        rb = GetComponent<Rigidbody2D>();
        passive = GetComponent<MinibossPassive>();
        playerTransform = GameObject.FindGameObjectsWithTag("Character")[0].transform;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasCollided)
        {
            currentCollisionCD -= Time.deltaTime;
            if(currentCollisionCD <= 0)
            {
                hasCollided = false;
                currentCollisionCD = maxCollisionCD;
            }
        }
        HandleDirection();

        if(currentHP <= 0)
        {
            currentState = BossState.DEAD;
        }

        switch(currentState)
        {
            case BossState.IDLE:
                HandleIdleState();
                HandleDirectionalAnimation("Idle", currentDirection);
                break;
            case BossState.NORMAL:
                if(isSecondNormal == false)
                    HandleNormalState();
                else
                    HandleSecondNormalState();
                HandleMovement(currentState);
                HandleDirectionalAnimation("Walk", currentDirection);
                break;
            case BossState.AGITATED:
                HandleAgitatedState();
                HandleMovement(currentState);
                HandleDirectionalAnimation("Run", currentDirection);
                break;
            case BossState.ATTACK:
                HandleAttackState();
                HandleDirectionalAnimation("Punch", currentDirection);
                break;
            case BossState.BEAM:
                HandleBeamState();
                break;
            case BossState.ULTIMATE:
                HandleUltimateState();
                break;
            case BossState.DEAD:
                HandleDead();
                break;
            default:
                break;
        }

        HandlePassive();
    }

    private void HandleMovement(BossState currState)
    {
        if (playerTransform == null)
            return;

        Debug.Log($"state={currState}, playerTransform={playerTransform}, isInRange={isInRange}, rb={rb}, moveSpeed={moveSpeed}");

        if (currState == BossState.NORMAL)
            moveSpeed = normalMoveSpeed;
        else if (currState == BossState.AGITATED)
            moveSpeed = agitatedMoveSpeed;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(!attackRange.GetIsInRange())
        {
            Vector3 moveDir = (playerTransform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }

    private void HandleDirection()
    {
        Vector2 facingVector = playerTransform.position - transform.position;

        float angle = Mathf.Atan2(facingVector.y, facingVector.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }

        if (angle >= 45f && angle < 135f)
        {
            currentDirection = FacingDirection.BACK;
        }
        else if (angle >= 135f && angle < 225f)
        {
            currentDirection = FacingDirection.LEFT;
        }
        else if (angle >= 225f && angle < 315f)
        {
            currentDirection = FacingDirection.FRONT;
        }
        else
        {
            currentDirection = FacingDirection.RIGHT;
        }
    }

    private void HandleDirectionalAnimation(string action, FacingDirection direction)
    {
        string dir = "";
        switch (direction)
        {
            case FacingDirection.FRONT:
                dir = "F";
                break;
            case FacingDirection.BACK:
                dir = "B";
                break;
            case FacingDirection.LEFT:
                dir = "L";
                break;
            case FacingDirection.RIGHT:
                dir = "R";
                break;
            default:
                dir = "F";
                break;
        }

        string stateName = dir + action;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            return;

        animator.Play(stateName);

        /*        string animationName = dir + action; // e.g. "L" + "Walk" = "LWalk"
                if (animationName == prevAnim) return;

                animator.Play(animationName);
                prevAnim = animationName;*/
    }

    private void HandleNormalState()
    {
        isSecondNormal = false;

        //Still has time but haven't hit player
        if (currentNormalStateTimer > 0f && !attackRange.GetIsInRange())
        {
            currentNormalStateTimer -= Time.deltaTime;
        }
        //Didn't hit player before timer
        if (currentNormalStateTimer <= 0f && !attackRange.GetIsInRange())
        {
            currentState = BossState.AGITATED;
            ResetNormalState();
        }

        //Has hit player before timer
        if(currentNormalStateTimer > 0f && attackRange.GetIsInRange())
        {
            StartAttackState(currentState);
            currentState = BossState.ATTACK;
            
            ResetNormalState();
        }
    }
    private void HandleSecondNormalState()
    {
        isSecondNormal = true;

        //Still has time but haven't hit player
        if (currentNormalStateTimer > 0f && !attackRange.GetIsInRange())
        {
            currentNormalStateTimer -= Time.deltaTime;
        }

        //Didn't hit player before timer
        if (currentNormalStateTimer <= 0f && !attackRange.GetIsInRange())
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);
            ResetNormalState();
        }
        //Has hit player before timer
        else if (currentNormalStateTimer > 0f && attackRange.GetIsInRange())
        {
            StartAttackState(currentState);
            currentState = BossState.ATTACK;
            ResetNormalState();
        }
    }

    private void ResetNormalState()
    {
        currentNormalStateTimer = maxNormalStateTimer;
        hasLandedHit = false;
    }
    private void HandleAgitatedState()
    {
        //Still has time but haven't hit player
        if (currentAgitatedStateTimer > 0f && !attackRange.GetIsInRange())
        {
            currentAgitatedStateTimer -= Time.deltaTime;
        }
        //Didn't hit player before timer
        if (currentAgitatedStateTimer <= 0f && !attackRange.GetIsInRange())
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);

            currentAgitatedStateTimer = maxAgitatedStateTimer;
            hasLandedHit = false;
        }

        //Has hit player before timer
        if (currentAgitatedStateTimer > 0f && attackRange.GetIsInRange())
        {
            isSecondNormal = true;
          
            StartAttackState(currentState);
            currentState = BossState.ATTACK;

            currentAgitatedStateTimer = maxAgitatedStateTimer;
            hasLandedHit = false;
        }
    }

    private void StartIdleState(BossState nextState)
    {
        currentState = BossState.IDLE;
        stateAfterIdle = nextState;
    }
    private void HandleIdleState()
    {
        currentIdleDuration -= Time.deltaTime;
        rb.linearVelocity = Vector2.zero;

        if (currentIdleDuration <= 0f)
        {
            currentState = stateAfterIdle;
            currentIdleDuration = maxIdleDuration;
        }
    }

    private void StartAttackState(BossState prevState)
    {
        stateBeforeAttack = prevState;
    }

    private void HandleAttackState()
    {
/*        if(stateBeforeAttack == BossState.NORMAL)
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);
        }
        else if(stateBeforeAttack == BossState.AGITATED)
        {
            StartIdleState(BossState.NORMAL);
        }*/
    }

    public void OnHitEvent()
    {
        if(attackRange.GetTarget().GetComponent<Character>())
        {
            attackRange.GetTarget().GetComponent<Character>().CharacterAttacked(attackDamage);
        }
        
    }

    public void OnHitEndEvent()
    {
        if (stateBeforeAttack == BossState.NORMAL)
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);
        }
        else if (stateBeforeAttack == BossState.AGITATED)
        {
            StartIdleState(BossState.NORMAL);
        }
    }
    private void HandleBeamState()
    {
        isSecondNormal = false;
        hasBeamed = true;

        if (!hasStartedBeam)
        {
            hasStartedBeam = true;

            string dir = currentDirection == FacingDirection.FRONT ? "F"
                       : currentDirection == FacingDirection.BACK ? "B"
                       : currentDirection == FacingDirection.LEFT ? "L" : "R";

            beamAttack.EnableBeamAttack(dir);
            return;
        }

        if (beamAttack.GetBeamEnd())
        {
            hasStartedBeam = false;
            StartIdleState(BossState.NORMAL);
        }
    }

    private void HandleUltimateState()
    {
        isSecondNormal = false;
        hasBeamed = false;

        if(!hasUltStarted)
        {
            string dir = currentDirection == FacingDirection.FRONT ? "F"
                       : currentDirection == FacingDirection.BACK ? "B"
                       : currentDirection == FacingDirection.LEFT ? "L" : "R";

            ultAttack.SetIsStartUlt(true, dir);
            ultAttack.SetIsEndUlt(false);
            hasUltStarted = true;
        }
        if(ultAttack.GetIsUltEnd())
        {
            StartIdleState(BossState.NORMAL);
            hasUltStarted = false;
        }
            

    }

    private void HandlePassive()
    {
        if (currentPassiveCooldown > 0f && isPassiveOn)
        {
            currentPassiveCooldown -= Time.deltaTime;
        }
        else if (currentPassiveCooldown <= 0f)
        {
            isPassiveOn = false;
            passive.TriggerSpawn();
            currentPassiveCooldown = maxPassiveCooldown;
            passive.completedSpawning.AddListener(HandlePassiveSpawnComplete);
        }
    }

    private void HandlePassiveSpawnComplete()
    {
        isPassiveOn = true;
    }

    private void HandleDead()
    {
        if(!hasDeathAnimPlayed)
        {
            bossDeath?.Invoke();
            HandleDirectionalAnimation("Hurt", currentDirection);
            moveSpeed = 0f;
            hasDeathAnimPlayed = true;
            passive.completedSpawning.RemoveListener(HandlePassiveSpawnComplete);
            isPassiveOn = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            hasCollided = true;
            collision.GetComponent<Character>().CharacterAttacked(collisionDamage);
        }
    }
}
