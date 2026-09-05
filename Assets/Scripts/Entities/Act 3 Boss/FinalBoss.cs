using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    private Character character;
    private DashAttack dashAttack;
    private Vector2 normalizedChase;
    //Player's last position
    public Vector2 lastPos;
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
    //Shielded state (Barrier active state)
    public bool shielded;
    //Dashing state
    public bool dashing;
    //Character last position
    public Vector3 characterPos;
    //Beam direction (False is right, true is left)
    public bool beamDirection;
    //Random probability for boss pattern
    public float randomNum;
    [SerializeField] private GameObject soldier;
    [SerializeField] private GameObject eyeLaser;
    [SerializeField] private GameObject beams;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject missileLauncher;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject suctionEffect;
    [SerializeField] private GameObject windEffect;
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private GameObject groundSmash;
    [SerializeField] private GameObject fireCannon;
    [SerializeField] private GameObject elementalProjectile;
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
        shielded = false;
        attackTime = 0f;
        randomNum = Random.Range(0f, 1f);
        defaultMaterial = sr.material;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = soldier.GetComponent<Character>();
        dashAttack = dash.GetComponent<DashAttack>();
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

        //Boss is alive and currently not attacking
        if(!death && !attacking)
        {
            //Flip sprite to face character
            if (character.transform.position.x > transform.position.x)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }

            if(Input.GetKeyDown(KeyCode.T))
            {
                Suction();
            }

            if(!Input.GetKeyDown(KeyCode.Y))
            {
                Laser();
            }

            if (!Input.GetKeyDown(KeyCode.G))
            {
                HomingMissile();
            }

            if (!Input.GetKeyDown(KeyCode.H))
            {
                FireCannon();
            }

            if (!Input.GetKeyDown(KeyCode.B))
            {
                Beams();
            }

            if (!Input.GetKeyDown(KeyCode.N))
            {
                ElementalProjectile();
            }

            //Random attack between fixed intervals
            if (attackTime > intervals)
            {
                attacking = true;

                ////If boss is far
                //if (Vector2.Distance(character.transform.position, transform.position) > 20f)
                //{
                //    if (randomNum < 0.5f)
                //    {
                //        Dash();
                //    }
                //    else if (randomNum < 0.6f)
                //    {
                //        Suction();
                //    }
                //    else if (randomNum < 0.7f)
                //    {
                //        Laser();
                //    }
                //    else if (randomNum < 0.8f)
                //    {
                //        HomingMissile();
                //    }
                //    else if(randomNum < 0.9f)
                //    {
                //        Beams();
                //    }
                //    else
                //    {
                //        FireCannon();
                //    }
                //}
                ////If boss is close
                //else
                //{
                //    if (randomNum > 0.5f)
                //    {
                //        Laser();
                //    }
                //    else if (randomNum < 0.7f)
                //    {
                //        Suction();
                //    }
                //    else if (randomNum < 0.9f)
                //    {
                //        HomingMissile();
                //    }
                //    else
                //    {
                //        Beams();
                //    }
                //}
            }
        }

        Barrier();
    }

    private void FixedUpdate()
    {
        //Boss died
        if(death)
        {
            //Stop moving
            rb.linearVelocity = Vector2.zero;
        }
        //Boss alive
        else
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
                    rb.linearVelocity = dashAttack.dashNormalized * dashAttack.dashSpeed;
                }
                //Stationary when attacking
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    //Increases movement speed for a period of time
    private void Dash()
    {
        animator.SetBool("dash", true);
        dash.SetActive(true);
    }

    //Shoot laser at player position
    private void Laser()
    {
        eyeLaser.SetActive(true);
    }

    //Missile that chases player and detonate upon collision or automatically by itself after a period of time
    private void HomingMissile()
    {
        missileLauncher.SetActive(true);
    }

    //Pulls player in before unleashing a shockwave nearby
    private void Suction()
    {
        animator.SetBool("attack", true);
        suctionEffect.SetActive(true);
        windEffect.SetActive(true);
        rangeIndicator.SetActive(true);
    }

    //4 directional beam that rotates in a full circle (Random starting rotation, random direction)
    private void Beams()
    {
        animator.SetBool("attack", true);
        beams.SetActive(true);
    }

    //Spawns ground impact attacks that chases player when smashing the ground
    private void GroundSmash()
    {
        animator.SetBool("smash", true);
        groundSmash.SetActive(true);
    }

    //Spray fire attack consecutively at player at close range
    private void FireCannon()
    {
        animator.SetBool("attack", true);
        fireCannon.SetActive(true);
    }

    //Shoot elemental projectile attacks consecutively at player
    private void ElementalProjectile()
    {
        animator.SetBool("attack", true);
        elementalProjectile.SetActive(true);
    }

    //Shield that reflects bullets
    private void Barrier()
    {
        //Barrier only appears at 10% chance when boss is less than 50% health
        if(bossHP < 500 && randomNum < 0.1f && !barrier.activeSelf)
        {
            shielded = true;
            barrier.SetActive(true);
        }
    }

    //Summon drones that hover at the left and right side of the viewport respectively
    private void SummonDrone()
    {
        leftDrone.SetActive(true);
        rightDrone.SetActive(true);
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
        if(collision.gameObject.CompareTag("Character") && !character.isAttacked && !shielded)
        {
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.1f);
        }
        else if (collision.gameObject.CompareTag("Sword") && !isAttacked && !shielded)
        {
            //Final boss flashes and take damage when attacked
            BossAttacked(50, 0.3f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Flamethrower") && !isAttacked && !shielded)
        {
            //Final boss flashes and take damage when attacked
            BossAttacked(5, 0.4f);
        }
    }
}