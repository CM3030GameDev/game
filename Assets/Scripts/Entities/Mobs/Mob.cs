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
    // Companion hits get their own cooldown (see CompanionAttacked)
    private float companionHitTimer;

    [SerializeField] private int maxHP = 100;
    private int currentHP;
    [SerializeField] private float chaseSpeed = 5f;
    [Header("Exp drop")]
    [Tooltip("Highest value first. The roll is paid out greedily, so tiers of 10/5/1 turn a 7 " +
             "into one blue and two greens. Order is enforced at startup.")]
    [SerializeField] private OrbTier[] orbTiers;
    [SerializeField] private int minExpDrop = 2;
    [SerializeField] private int maxExpDrop = 4;
    [Tooltip("Orbs and pickups scatter within this radius so a drop does not stack on one pixel.")]
    [SerializeField] private float dropSpread = 0.6f;
    [Tooltip("Rolled independently per entry, so two pickups can drop from the same kill.")]
    [SerializeField] private PickupDrop[] pickupDrops;
    [SerializeField] private float separationRadius = 0.6f;
    [SerializeField] private float separationStrength = 2f;
    [SerializeField] private float deathAnimationDuration = 0.5f;
    [Header("Contact damage")]
    [SerializeField] private int contactDamage = 10;
    [Tooltip("Seconds the player is immune after touching any enemy.")]
    [SerializeField] private float contactInvulnerability = 0.5f;
    [Header("Companion damage")]
    [SerializeField] private int swordDamage = 20;
    [SerializeField] private float swordCooldown = 0.5f;
    [SerializeField] private int flamethrowerDamage = 5;
    [SerializeField] private float flamethrowerCooldown = 0.25f;

    [System.Serializable]
    public struct OrbTier
    {
        public GameObject prefab;
        public int value;
    }

    [System.Serializable]
    public struct PickupDrop
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float chance;
    }

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

        // Greedy payout only works from the largest denomination down, so don't rely on
        // whoever filled the array in the Inspector getting the order right.
        if (orbTiers != null) System.Array.Sort(orbTiers, (a, b) => b.value.CompareTo(a.value));
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
        companionHitTimer = 0f;
    }

    private void OnDisable()
    {
        // Only drop exp on a real death, since OnDisable also fires when the pool is built.
        if (!death) return;

        DropExp();
        DropPickups();
    }

    private void DropPickups()
    {
        if (pickupDrops == null) return;

        foreach (PickupDrop drop in pickupDrops)
        {
            if (drop.prefab == null || Random.value > drop.chance) continue;
            Spawn(drop.prefab);
        }
    }

    // Rolls an exp budget and pays it out in the largest orbs that fit, so one number per mob
    // decides both how much exp it is worth and which orbs it drops.
    private void DropExp()
    {
        if (orbTiers == null) return;

        int remaining = Random.Range(minExpDrop, maxExpDrop + 1);

        foreach (OrbTier tier in orbTiers)
        {
            if (tier.prefab == null || tier.value <= 0) continue;

            int count = remaining / tier.value;
            remaining -= count * tier.value;

            for (int i = 0; i < count; i++) Spawn(tier.prefab);
        }
    }

    private void Spawn(GameObject prefab)
    {
        Vector2 offset = Random.insideUnitCircle * dropSpread;
        Instantiate(prefab, (Vector2)transform.position + offset, Quaternion.identity);
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

        if (companionHitTimer > 0f) companionHitTimer -= Time.deltaTime;

        // Debuff timer that ticks down
        if (debuffTimer > 0f)
        {
            debuffTimer -= Time.deltaTime;
            if (debuffTimer <= 0f) speedMultiplier = 1f;
        }
    }
    public void SetDead()
    {
        if (!death)
        {
            animator.SetTrigger("dead");
            death = true;
            Invoke(nameof(Despawn), deathAnimationDuration);
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

    // Stay, not Enter, so standing in a crowd keeps hurting instead of only the first touch.
    private void OnTriggerStay2D(Collider2D other)
    {
        // A mob playing its death animation still has its collider, so it must not keep hurting
        if (death) return;

        if (other.CompareTag("Character"))
            other.GetComponent<Character>()?.CharacterAttacked(contactDamage, contactInvulnerability);
        else if (other.CompareTag("Sword"))
            CompanionAttacked(swordDamage, swordCooldown);
        else if (other.CompareTag("Flamethrower"))
            CompanionAttacked(flamethrowerDamage, flamethrowerCooldown);
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

    // Its own cooldown, because player bullets hold isAttacked down and would eat these hits.
    public void CompanionAttacked(int amount, float cooldown)
    {
        if (companionHitTimer > 0f) return;
        companionHitTimer = cooldown;
        UnblockedAttack(amount, 0.1f);
    }

    // Damage and flash without taking the isAttacked lock, for sources that limit themselves.
    public void UnblockedAttack(int amount, float flash)
    {
        if (death) return;
        currentHP -= amount;
        animator.SetBool("attacked", true);
        attackedTime = Mathf.Max(attackedTime, flash);
    }

    public bool WouldDie(int amount) => currentHP <= amount;

    // Bypasses the isAttacked guard, for effects that must land on every enemy.
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
            // Clamp the distance so stacked mobs do not get a huge push from dividing by zero.
            float dist = Mathf.Max(away.magnitude, 0.1f);
            push += away.normalized / dist; // Push enemies away from each other the closer they are
        }

        return push;
    }
}
