using UnityEngine;
using UnityEngine.Events;

public abstract class MobBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected BoxCollider2D box;
    protected SpriteRenderer sr;
    protected Animator animator;
    protected GameObject character;

    //protected int enemyHP;
    protected Vector2 chaseDirection;
    protected Vector2 normalizedChase;

    protected bool death;
    protected bool isAttacked;
    protected float attackedTime;
    protected float attackedDuration;

    protected float debuffTimer;
    protected float speedMultiplier = 1f;
    protected int currentHP;

    [SerializeField] protected GameObject expOrbPrefab;

    protected abstract float ChaseSpeed { get; }
    protected abstract int ExpReward { get; }
    protected abstract float SeparationRadius { get; }
    protected abstract float SeparationStrength { get; }
    protected abstract int MaxHP { get; }
    protected abstract float AttackRange { get; }
    protected abstract float AttackCooldown { get; }

    public UnityEvent onDeath;
    protected virtual void Awake()
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
    protected virtual void Start()
    {

    }

    protected virtual void OnEnable()
    {
        isAttacked = false;
        death = false;
        currentHP = MaxHP;
    }

    // Update is called once per frame
    protected virtual void Update()
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

            Despawn(); //temp function to despawn the enemy, remove this later when we add death animations that reference this!!!
        }

        // Debuff timer that ticks down
        if (debuffTimer > 0f)
        {
            debuffTimer -= Time.deltaTime;
            if (debuffTimer <= 0f) speedMultiplier = 1f;
        }
    }

    public virtual void Despawn()
    {
        // Drops exp orb at mob position when the mob is dead
        if (expOrbPrefab != null)
        {
            GameObject orb = Instantiate(expOrbPrefab, transform.position, Quaternion.identity);
            orb.GetComponent<ExpOrb>().SetExp(ExpReward);
        }

        onDeath?.Invoke();

        //Return to pool
        gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate()
    {
        //Enemy alive
        if (currentHP > 0)
        {
            Vector2 chase = normalizedChase;
            Vector2 separation = GetSeparation() * SeparationStrength;
            Vector2 move = (chase + separation).normalized;

            rb.linearVelocity = move * ChaseSpeed * speedMultiplier;
        }
        //Enemy dead
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
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

    protected virtual void OnTriggerStay2D(Collider2D other)
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            character.CharacterAttacked(10);
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

    public virtual void MobAttacked(int amount, float timer)
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

    public virtual void ApplyDebuff(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        debuffTimer = Mathf.Max(debuffTimer, duration);
    }

    protected virtual Vector2 GetSeparation()
    {
        Vector2 push = Vector2.zero;
        Collider2D[] neighbours = Physics2D.OverlapCircleAll(transform.position, SeparationRadius);

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
