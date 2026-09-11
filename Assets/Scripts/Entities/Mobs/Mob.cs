using UnityEngine;
using UnityEngine.Events;

public class Mob : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private SpriteRenderer sr;
    private Animator animator;
    private GameObject character;

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

    [SerializeField] private int maxHP = 100;
    private int currentHP;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private GameObject expOrbPrefab;
    [SerializeField] private int expReward = 10;   // Flat value for exp (change later!!)
    [SerializeField] private float separationRadius = 0.6f;
    [SerializeField] private float separationStrength = 2f;
    [SerializeField] private float deathAnimationDuration = 0.5f;

    public UnityEvent onDeath;
    private void Awake()
    {
        EnemyMask = LayerMask.GetMask("Enemy");
        death = false;
        isAttacked = false;
        attackedTime = 0f;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        character = GameObject.FindGameObjectsWithTag("Character")[0];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    private void OnEnable()
    {
        CancelInvoke(nameof(Despawn)); // clear any leftover invoke from an instantKillAllActive() interrupting a death in progress
        isAttacked = false;
        death = false;
        currentHP = maxHP;
        knockbackTimer = 0f;
    }

    private void OnDisable()
    {
        // Only drop exp if this mob actually died. OnDisable also fires when MobManager builds
        // the pool at startup (Instantiate -> SetActive(false)) and when the scene unloads,
        // both of which would otherwise spawn phantom orbs.
        if (!death) return;

        // Drops exp orb at mob position when the mob is dead
        if (expOrbPrefab != null)
        {
            GameObject orb = Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<ExpOrb>().SetExp(expReward);
        }
    }

    // Update is called once per frame
    void Update()
    {
        box.size = sr.sprite.bounds.size;

        chaseDirection = character.transform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Flip sprite according to aim position
        if (character.transform.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
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


        //Mob dead
        if (currentHP <= 0 && !death)
        {
            //Death animation & automatically destroys mob
            animator.SetTrigger("dead");
            death = true;
            Invoke(nameof(Despawn), deathAnimationDuration);
        }

        // Debuff timer that ticks down
        if (debuffTimer > 0f)
        {
            debuffTimer -= Time.deltaTime;
            if (debuffTimer <= 0f) speedMultiplier = 1f;
        }
    }

    public void Despawn()
    {
        // Account for death of mob
        onDeath?.Invoke();
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
                Vector2 chase = normalizedChase;
                Vector2 separation = GetSeparation() * separationStrength;
                Vector2 move = (chase + separation).normalized;

                rb.linearVelocity = move * chaseSpeed * speedMultiplier;
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

    public bool WouldDie(int amount) => currentHP <= amount;

    // Bypasses the isAttacked guard - for effects that must land on every enemy
    // regardless of hit-flash state (e.g. a screen-clearing panic skill).
    public void GuaranteedAttack(int amount, float timer)
    {
        animator.SetBool("attacked", true);
        isAttacked = true;
        attackedTime = timer;
        currentHP -= amount;
    }

    public void ApplyDebuff(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        debuffTimer = Mathf.Max(debuffTimer, duration);
    }

    public void Knockback(Vector2 direction, float force)
    {
        knockbackVelocity = direction.normalized * force;
        knockbackTimer = 0.15f;
    }

    private static int EnemyMask;
    private static readonly Collider2D[] SeparationBuffer = new Collider2D[16];

    private Vector2 GetSeparation()
    {
        Vector2 push = Vector2.zero;
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, separationRadius, SeparationBuffer, EnemyMask);

        for (int i = 0; i < count; i++)
        {
            Collider2D n = SeparationBuffer[i];
            if (n.gameObject == gameObject) continue;

            Vector2 away = (Vector2)transform.position - (Vector2)n.transform.position;
            // Clamp the distance used here so nearly-overlapping mobs (e.g. a wave spawning
            // stacked together) don't get an explosive push force from dividing by ~0.
            float dist = Mathf.Max(away.magnitude, 0.1f);
            push += away.normalized / dist; // Push enemies away from each other the closer they are
        }

        return push;
    }
}
