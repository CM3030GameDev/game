using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Act2Miniboss : MonoBehaviour
{
    private enum BossState
    {
        IDLE,
        NORMAL,
        AGITATED,
        BEAM,
        ULTIMATE
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

    //the sprites are all offset a little because of the height difference in sprite, use this as a point instead to get direction
    [SerializeField] private Transform pivotTransform;

    [Header("Minion Passive")]
    [SerializeField] private GameObject minionObject;
    [SerializeField] private int numberOfMinions = 5;
    [SerializeField] private int gridSpacing = 5;
    [SerializeField] private float spawnInterval = 1f;
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

    private float topOffset = 0f;
    private float sideOffset = 0f;

    [Header("Movement")]
    [SerializeField] private float normalMoveSpeed = 5f;
    [SerializeField] private float agitatedMoveSpeed = 8f;
    [SerializeField] private float stopDistance = 1f;
    private float moveSpeed = 0f;
    private Transform playerTransform;

    private MinibossBeamAttack beamAttack;

    [SerializeField] private Animator animator;
    private string prevAnim = "";

    private void Awake()
    {
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //minionCoroutine = StartCoroutine(SpawnMinions());
        currentAgitatedStateTimer = maxAgitatedStateTimer;
        currentNormalStateTimer = maxNormalStateTimer;
        currentIdleDuration = maxIdleDuration;
        currentPassiveCooldown = maxPassiveCooldown;
        currentState = BossState.NORMAL;
        isPassiveOn = true;
        beamAttack = this.GetComponent<MinibossBeamAttack>();
        playerTransform = GameObject.FindGameObjectsWithTag("Character")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {

        HandleDirection();

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
            case BossState.BEAM:
                HandleBeamState();
                break;
            case BossState.ULTIMATE:
                HandleUltimateState();
                break;
            default:
                break;
        }

        HandlePassive();
    }

    //do i need to reset movespeed to 0
    private void HandleMovement(BossState currState)
    {
        if (playerTransform == null)
            return;

        if (currState == BossState.NORMAL)
            moveSpeed = normalMoveSpeed;
        else if (currState == BossState.AGITATED)
            moveSpeed = agitatedMoveSpeed;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if(distance > stopDistance)
        {
            Vector3 moveDir = (playerTransform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
    private void HandleDirection()
    {
        Vector2 facingVector = playerTransform.position - pivotTransform.position;

        float angle = Mathf.Atan2(facingVector.y, facingVector.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }
        Debug.Log($"facingVector: {facingVector}, angle: {angle}, chosen: {currentDirection}"); // ADD THIS
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

        // Check the Animator's ACTUAL current state, not a cached guess
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
        if (currentNormalStateTimer > 0f && !hasLandedHit)
        {
            currentNormalStateTimer -= Time.deltaTime;
        }
        //Didn't hit player before timer
        if (currentNormalStateTimer <= 0f && !hasLandedHit)
        {
            currentState = BossState.AGITATED;
            ResetNormalState();
        }

        //Has hit player before timer
        if(currentNormalStateTimer > 0f && hasLandedHit)
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);

            ResetNormalState();
        }
    }
    private void HandleSecondNormalState()
    {
        isSecondNormal = true;

        //Still has time but haven't hit player
        if (currentNormalStateTimer > 0f && !hasLandedHit)
        {
            currentNormalStateTimer -= Time.deltaTime;
        }
        //Didn't hit player before timer
        if (currentNormalStateTimer <= 0f && !hasLandedHit || currentNormalStateTimer > 0f && hasLandedHit)
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);

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
        if (currentAgitatedStateTimer > 0f && !hasLandedHit)
        {
            currentAgitatedStateTimer -= Time.deltaTime;
        }
        //Didn't hit player before timer
        if (currentAgitatedStateTimer <= 0f && !hasLandedHit)
        {
            StartIdleState(hasBeamed ? BossState.ULTIMATE : BossState.BEAM);

            currentAgitatedStateTimer = maxAgitatedStateTimer;
            hasLandedHit = false;
        }

        //Has hit player before timer
        if (currentAgitatedStateTimer > 0f && hasLandedHit)
        {
            isSecondNormal = true;
            StartIdleState(BossState.NORMAL);

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

/*        //resetbeam
        hasStartedBeam = false;*/

        if(currentIdleDuration <= 0f)
        {
            currentState = stateAfterIdle;
            currentIdleDuration = maxIdleDuration;
        }
    }

    private float maxtemptimer = 5f;
    private float temptimer = 0f;
    private bool hasStartedBeam = false;

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
            return; // don't check GetBeamEnd on the same frame we just started it
        }

        if (beamAttack.GetBeamEnd())
        {
            hasStartedBeam = false; // <-- THIS is what was missing. Resets it for next time.
            StartIdleState(BossState.NORMAL);
        }
    }

    private void HandleUltimateState()
    {
        isSecondNormal = false;

        hasBeamed = false;
        if (temptimer < maxtemptimer)
        {
            temptimer += Time.deltaTime;
        }
        else
        {
            StartIdleState(BossState.NORMAL);
            temptimer = 0f;
        }
    }

    private void HandlePassive()
    {
        if(currentPassiveCooldown > 0f && isPassiveOn)
        {
            currentPassiveCooldown -= Time.deltaTime;
        }
        else if(currentPassiveCooldown <= 0f)
        {
            isPassiveOn = false;
            StartCoroutine(SpawnMinions());
            currentPassiveCooldown = maxPassiveCooldown;
        }
    }

    private IEnumerator SpawnMinions()
    {
        if(!hasPlayerPos)
        {
            GameObject player = GameObject.FindGameObjectsWithTag("Character")[0];
            playerPos = player.transform.position;
            hasPlayerPos = true;
            topOffset = (numberOfMinions - 1) * gridSpacing / 2f;
            sideOffset = (numberOfMinions - 1) * gridSpacing / 2f;
        }

        for (int col = 1; col <= numberOfMinions; col++)
        {
            float xOffset = (col - 1) * gridSpacing - topOffset; // shift left by startOffset
            GameObject y = Instantiate(minionObject, playerPos + new Vector2(xOffset, topOffset), Quaternion.identity);
            y.GetComponent<Minion>().Initialize(Minion.DirectionFacing.TOP);
            yield return new WaitForSeconds(spawnInterval);
        }

        for (int row = 1; row <= numberOfMinions; row++)
        {
            float yOffset = (row * gridSpacing) - sideOffset;
            GameObject x = Instantiate(minionObject, playerPos + new Vector2((-sideOffset) - gridSpacing, -yOffset), Quaternion.identity);
            x.GetComponent<Minion>().Initialize(Minion.DirectionFacing.LEFT);
            yield return new WaitForSeconds(spawnInterval);
        }

        hasPlayerPos = false;
        isPassiveOn = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Character") && currentState != BossState.IDLE)
        {
            hasLandedHit = true;
        }
    }
}
