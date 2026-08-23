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
    //Attacked state duration
    private float attackedDuration;
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

    public UnityEvent onDeath;
    private void Awake()
    {
        death = false;
        isAttacked = false;
        attackedTime = 0f;
        attackedDuration = 0f;
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
        isAttacked = false;
        death = false;
        currentHP = maxHP;
        knockbackTimer = 0f;
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
        attackedTime += Time.deltaTime;

        //Enemy can be attacked again
        if (attackedTime >= attackedDuration)
        {
            isAttacked = false;
            animator.SetBool("attacked", false);
        }

        //Enemy dead
        if (currentHP <= 0 && !death)
        {
            animator.SetTrigger("dead");
            death = true;
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
        // Drops exp orb at mob position when the mob is dead
        if (expOrbPrefab != null)
        {
            GameObject orb = Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<ExpOrb>().SetExp(expReward);
        }

        // Account for death of mob
        onDeath?.Invoke();

        //Return to pool
        gameObject.SetActive(false);
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
        if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(10);
        }

        if (collision.CompareTag("Sword") && !isAttacked)
        {
            //Mob flashes when attacked
            animator.SetBool("attacked", true);
            isAttacked = true;
            attackedTime = 0f;
            attackedDuration = 0.2f;
            currentHP -= 20;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            character.CharacterAttacked(10);
            character.GrantInvulnerability(0.1f);
        }

        if (collision.gameObject.CompareTag("Sword") && !isAttacked)
        {
            //Mob flashes when attacked
            animator.SetBool("attacked", true);
            isAttacked = true;
            attackedTime = 0f;
            attackedDuration = 0.2f;
            currentHP -= 20;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Flamethrower") && !isAttacked)
        {
            //Mob flashes when attacked
            animator.SetBool("attacked", true);
            isAttacked = true;
            attackedTime = 0f;
            attackedDuration = 0.4f;
            currentHP -= 5;
        }
    }

    public void MobAttacked(int amount, float timer)
    {
        if (!isAttacked)
        {
            //Mob flashes when attacked
            animator.SetBool("attacked", true);
            isAttacked = true;
            attackedTime = 0f;
            attackedDuration = timer;
            currentHP -= amount;
        }
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

    private Vector2 GetSeparation()
    {
        Vector2 push = Vector2.zero;
        Collider2D[] neighbours = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (var n in neighbours)
        {
            if (n.gameObject == gameObject) continue;
            if (!n.CompareTag("Enemy")) continue;

            Vector2 away = (Vector2)transform.position - (Vector2)n.transform.position;
            float dist = away.magnitude;
            if (dist > 0.01f)
                push += away.normalized / dist; // Push enemies away from each other the closer they are
        }

        return push;
    }
}
