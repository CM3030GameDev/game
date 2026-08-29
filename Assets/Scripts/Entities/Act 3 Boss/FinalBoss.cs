using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    private Character character;
    private DashAttack dashAttack;
    private Vector2 normalizedChase;
    //Attacked state
    private bool isAttacked;
    //Invulnerable timer
    private float invulnTime;
    //Attack timer
    public float attackTime;
    //Death state
    private bool death;
    //Attacking state
    public bool attacking;
    //Dashing state
    public bool dashing;
    //Beam direction (False is right, true is left)
    public bool beamDirection;
    //Random probability for boss pattern
    public float randomNum;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject eyeLaser;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject missileLauncher;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject suction;
    [SerializeField] private GameObject wind;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject leftDrone;
    [SerializeField] private GameObject rightDrone;
    //Default material
    private Material defaultMaterial;
    //White material
    [SerializeField] private Material whiteMaterial;
    //Boss HP
    [SerializeField] private int bossHP;
    //Boss speed
    [SerializeField] private int bossSpeed;
    //Boss damage
    [SerializeField] private int damage;
    //Interval timer
    [SerializeField] private float intervals;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attacking = false;
        isAttacked = false;
        attackTime = 0f;
        randomNum = Random.Range(0f, 1f);
        defaultMaterial = sr.material;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = soldier.GetComponent<Character>();
        dashAttack = dash.GetComponent<DashAttack>();

        leftDrone.SetActive(true);
        rightDrone.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTime < intervals)
        {
            attackTime += Time.deltaTime;
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

        //Boss dead
        if (bossHP <= 0 && !death)
        {
            animator.SetTrigger("dead");
            death = true;
        }

        Vector2 chaseDirection = character.transform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Flip sprite to face character
        if(!attacking)
        {
            if (character.transform.position.x > transform.position.x)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }

            if(attackTime > intervals)
            {
                attacking = true;

                //If boss is far
                if (Vector2.Distance(character.transform.position, transform.position) > 20f)
                {
                    if (randomNum < 0.5f)
                    {
                        Dash();
                    }
                    else if (randomNum < 0.7f)
                    {
                        Suction();
                    }
                    else if(randomNum < 0.8f)
                    {
                        Laser();
                    }
                    else if (randomNum < 0.9f)
                    {
                        Missile();
                    }
                    else
                    {
                        Beam();
                    }
                }
                //If boss is close
                else
                {
                    if (randomNum > 0.5f)
                    {
                        Laser();
                    }
                    else if (randomNum < 0.7f)
                    {
                        Suction();
                    }
                    else if (randomNum < 0.9f)
                    {
                        Missile();
                    }
                    else
                    {
                        Beam();
                    }
                }
            }
        }

        Barrier();
    }

    private void FixedUpdate()
    {
        //Move towards player
        if (!attacking)
        {
            rb.linearVelocity = normalizedChase * bossSpeed;
        }
        else
        {
            //Dash towards player
            if (dashing)
            {
                rb.linearVelocity = dashAttack.dashDirection * dashAttack.dashSpeed;
            }
            //Stationary
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void Dash()
    {
        animator.SetBool("dash", true);
        dash.SetActive(true);
    }

    private void Laser()
    {
        eyeLaser.SetActive(true);
    }

    private void Missile()
    {
        missileLauncher.SetActive(true);
    }

    private void Suction()
    {
        suction.SetActive(true);
        wind.SetActive(true);
        range.SetActive(true);
    }

    private void Beam()
    {
        animator.SetTrigger("beam_start");
    }

    private void Barrier()
    {
        //Barrier only appears at 10% chance when boss is less than 50% health
        if(bossHP < 500 && randomNum < 0.1f && !barrier.activeSelf)
        {
            barrier.SetActive(true);
        }
    }

    public void BossAttacked(int amount, float seconds)
    {
        if (!isAttacked)
        {
            // Final boss flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            bossHP -= amount;
            invulnTime = seconds;
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