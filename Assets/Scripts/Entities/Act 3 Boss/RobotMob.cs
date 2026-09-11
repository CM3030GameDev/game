using UnityEngine;

public class RobotMob : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private GameObject character;
    //Robot mob current health
    private int currentHP;
    //Vector direction towards player
    private Vector2 chaseDirection;
    //Normalized vector direction towards player
    private Vector2 normalizedChase;
    //Death state
    private bool death;
    //Attacked state
    private bool isAttacked;
    //Attacked state timer
    private float attackedTime;
    // Debuff (slow) from fire floor
    private float debuffTimer;
    private float speedMultiplier = 1f;
    // Knockback (from shotgun etc.)
    private Vector2 knockbackVelocity;
    private float knockbackTimer;
    [SerializeField] private int robotHP;
    [SerializeField] private int robotSpeed;
    [SerializeField] private GameObject expOrbPrefab;
    [SerializeField] private int expReward;

    private void Awake()
    {
        death = false;
        isAttacked = false;
        attackedTime = 0f;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        character = GameObject.FindGameObjectsWithTag("Character")[0];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    private void OnEnable()
    {
        isAttacked = false;
        death = false;
        currentHP = robotHP;
        knockbackTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        chaseDirection = character.transform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Flip sprite according to aim position
        if (character.transform.position.x > transform.position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        //Mob cannot be attacked
        if (attackedTime > 0f)
        {
            attackedTime -= Time.deltaTime;
        }
        //Mob can be attacked
        else
        {
            isAttacked = false;
            animator.SetBool("attacked", false);
        }

        //Robot mob alive
        if (!death)
        {
            //Robot mob dead
            if(currentHP <= 0)
            {
                death = true;
                //Start death animation of robot mob
                animator.SetTrigger("death");
            }
        }
        //Death explosion animation
        else
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Explosion") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                //Disable robot mob
                gameObject.SetActive(false);
                //Return robot mob to object pool
                //ActThreeMobManager.Instance.robots.Enqueue(gameObject);

                //Drop exp orb
                GameObject orb = Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
                orb.GetComponent<ExpOrb>().SetExp(expReward);
            }
        }

        // Debuff timer that ticks down
        if (debuffTimer > 0f)
        {
            debuffTimer -= Time.deltaTime;
            if (debuffTimer <= 0f) speedMultiplier = 1f;
        }
    }

    private void FixedUpdate()
    {
        //Enemy alive
        if (currentHP > 0)
        {
            if (knockbackTimer > 0f)
            {
                // Knockback to counteract normal movement while active
                knockbackTimer -= Time.fixedDeltaTime;
                rb.linearVelocity = knockbackVelocity;
            }
            else
            {
                rb.linearVelocity = normalizedChase * robotSpeed * speedMultiplier;
            }
        }
        //Enemy dead
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword") && !isAttacked)
        {
            MobAttacked(20, 0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if(!character.isAttacked)
            {
                character.CharacterAttacked(10);
                character.GrantInvulnerability(0.1f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Flamethrower") && !isAttacked)
        {
            MobAttacked(5, 0.4f);
        }
    }

    public void MobAttacked(int amount, float timer)
    {
        if (!isAttacked)
        {
            //Mob flashes when attacked
            animator.SetBool("attacked", true);
            isAttacked = true;
            attackedTime = timer;
            currentHP -= amount;
        }
    }

    //public bool WouldDie(int amount) => currentHP <= amount;

    //// Bypasses the isAttacked guard - for effects that must land on every enemy
    //// regardless of hit-flash state (e.g. a screen-clearing panic skill).
    //public void GuaranteedAttack(int amount, float timer)
    //{
    //    animator.SetBool("attacked", true);
    //    isAttacked = true;
    //    attackedTime = timer;
    //    currentHP -= amount;
    //}

    //public void ApplyDebuff(float multiplier, float duration)
    //{
    //    speedMultiplier = multiplier;
    //    debuffTimer = Mathf.Max(debuffTimer, duration);
    //}

    //public void Knockback(Vector2 direction, float force)
    //{
    //    knockbackVelocity = direction.normalized * force;
    //    knockbackTimer = 0.15f;
    //}

}

