using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private GameObject character;
    //Vector direction towards player
    private Vector2 chaseDirection;
    //Normalized vector direction towards player
    private Vector2 normalizedChase;
    //Attacked state
    private bool isAttacked;
    //Attacked state timer
    private float attackedTime;
    //Attacked state duration
    private float attackedDuration;
    //Boss HP
    private int bossHP;
    //Death state
    private bool death;
    //Attacking state
    public bool attacking;
    //Beam direction (False is right, true is left)
    public bool beamDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bossHP = 1000;
        attacking = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GameObject.FindGameObjectsWithTag("Character")[0];
    }

    // Update is called once per frame
    void Update()
    {
        chaseDirection = character.transform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Flip sprite to face character
        if (character.transform.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        attackedTime += Time.deltaTime;

        ////Boss can be attacked again
        //if (attackedTime >= attackedDuration)
        //{
        //    isAttacked = false;
        //    animator.SetBool("attacked", false);
        //}

        //Boss dead
        if (bossHP <= 0 && !death)
        {
            animator.SetTrigger("dead");
            death = true;
        }

        //For testing purpose only
        if(Input.GetKeyDown(KeyCode.R))
        {
            Beam();
        }
    }

    private void Beam()
    {
        if(!attacking)
        {
            attacking = true;
            //Beam starts from right
            if (character.transform.position.x > transform.position.x)
            {
                beamDirection = false;
            }
            //Beam starts from left
            else
            {
                beamDirection = true;
            }
            animator.SetTrigger("beam_start");
        }
    }
}
