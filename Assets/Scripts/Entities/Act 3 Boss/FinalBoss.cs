using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private Character character;
    //Attacked state
    private bool isAttacked;
    //Invulnerable timer
    private float invulnTime;
    //Reflect state
    public bool isReflect;
    //Barrier timer
    private float barrierTime;
    //Death state
    private bool death;
    //Attacking state
    public bool attacking;
    //Charging state
    private bool charging;
    //Dashing state
    private bool dashing;
    //Character position
    private Vector2 characterPos;
    //Dash direction
    private Vector2 dashDirection;
    //Beam direction (False is right, true is left)
    public bool beamDirection;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject barrier;
    //Default material
    private Material defaultMaterial;
    //White material
    [SerializeField] private Material whiteMaterial;
    //Boss HP
    [SerializeField] private int bossHP;
    //Boss damage
    [SerializeField] private int damage;
    [SerializeField] private int dashSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attacking = false;
        isAttacked = false;
        isReflect = false;
        defaultMaterial = sr.material;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = soldier.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        //chaseDirection = character.transform.position - transform.position;
        //normalizedChase = chaseDirection.normalized;

        //Flip sprite to face character
        if (character.transform.position.x > transform.position.x && !attacking)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        //Boss cannot be attacked
        if (invulnTime > 0)
        {
            invulnTime -= Time.deltaTime;
        }
        //Boss can be attacked
        else
        {
            //Change back to default material
            sr.material = defaultMaterial;
            isAttacked = false;
        }

        //Dashing towards player
        if (charging && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            charging = false;
            dashing = true;
            //Dash towards player current position after charging
            characterPos = character.transform.position;
            dashDirection = (character.transform.position - transform.position).normalized;
        }

        if (dashing && (Vector2)transform.position == characterPos)
        {
            dashing = false;
            animator.SetBool("dash", false);
            rb.linearVelocity = Vector2.zero;
        }

        Barrier();

        //Barrier activated
        if (barrierTime > 0)
        {
            barrierTime -= Time.deltaTime;
        }
        //Barrier de-activated
        else
        {
            isReflect = false;
            barrier.SetActive(false);
        }

        //Boss dead
        if (bossHP <= 0 && !death)
        {
            animator.SetTrigger("dead");
            death = true;
        }

        //For testing purpose only
        if(Input.GetKeyDown(KeyCode.F))
        {
            Beam();
        }

        //For testing purpose only
        if (Input.GetKeyDown(KeyCode.G))
        {
            Dash();
        }
    }

    private void FixedUpdate()
    {
        if(dashing)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
        }
    }

    private void Dash()
    {
        if(Vector2.Distance(character.transform.position, transform.position) > 5f && !dashing && !charging)
        {
            charging = true;
            animator.SetBool("dash", true);
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

    private void Barrier()
    {
        //Barrier only appears at 5% chance when boss is less than 50% health
        if(bossHP < 500 && Random.Range(0f, 1f) <= 0.05f)
        {
            isReflect = true;
            barrier.SetActive(true);
            barrierTime = 10f;
        }
    }

    public void BossAttacked(int amount, float duration)
    {
        if (!isAttacked)
        {
            // Final boss flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            bossHP -= amount;
            invulnTime = duration;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Character") && !character.isAttacked)
        {
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.1f);
        }
        else if (collision.gameObject.CompareTag("Sword") && !isAttacked)
        {
            //Final boss flashes and take damage when attacked
            BossAttacked(50, 0.3f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Flamethrower") && !isAttacked)
        {
            //Final boss flashes and take damage when attacked
            BossAttacked(5, 0.4f);
        }
    }
}