using UnityEngine;

public class Mob : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private SpriteRenderer sr;
    private Animator animator;
    private GameObject character;
    //Enemy HP
    private int enemyHP;
    //Vector direction towards player
    private Vector2 chaseDirection;
    //Normalized vector direction towards player
    private Vector2 normalizedChase;
    //Blue mob (Combat mob)
    private bool isBlue;
    //Red mob (Range mob)
    private bool isRed;
    //Green mob (Elite mob)
    private bool isGreen;
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
    [SerializeField] private RuntimeAnimatorController blueMob;
    [SerializeField] private RuntimeAnimatorController redMob;
    [SerializeField] private RuntimeAnimatorController greenMob;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float separationRadius = 0.6f;
    [SerializeField] private float separationStrength = 2f;

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
        isBlue = false;
        isRed = false;
        isGreen = false;
        isAttacked = false;
        death = false;

        //Types of enemy mobs for act 1
        if(sceneState.act == 1)
        {
            if (!enemySystem.enemySpawn)
            {
                isBlue = true;
            }
            else
            {
                float randomNum = Random.Range(0f, 1f);
                if(randomNum < 0.5f)
                {
                    isBlue = true;
                }
                else
                {
                    isRed = true;
                }
            }
        }
        //Types of enemy mobs for act 2
        else
        {
            if (!enemySystem.enemySpawn)
            {
                float randomNum = Random.Range(0f, 3f);
                if (randomNum < 1f)
                {
                    isBlue = true;
                }
                else if(randomNum < 2f)
                {
                    isRed = true;
                }
                else
                {
                    isGreen = true;
                }
            }
            else
            {
                float randomNum = Random.Range(0f, 1f);
                if (randomNum < 0.4f)
                {
                    isRed = true;
                }
                else
                {
                    isGreen = true;
                }
            }
        }

        if (isBlue)
        {
            animator.runtimeAnimatorController = blueMob;
            enemyHP = 100;
        }
        else if (isRed)
        {
            animator.runtimeAnimatorController = redMob;
            enemyHP = 50;
        }
        else if (isGreen)
        {
            animator.runtimeAnimatorController = greenMob;
            enemyHP = 100;
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
        attackedTime += Time.deltaTime;

        //Enemy can be attacked again
        if (attackedTime >= attackedDuration)
        {
            isAttacked = false;
            animator.SetBool("attacked", false);
        }

        //Enemy dead
        if (enemyHP <= 0 && !death)
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

    private void FixedUpdate()
    {
        //Enemy alive
        if (enemyHP > 0)
        {
            Vector2 chase = normalizedChase;
            Vector2 separation = GetSeparation() * separationStrength;
            Vector2 move = (chase + separation).normalized;

            rb.linearVelocity = move * chaseSpeed * speedMultiplier;
        }
        //Enemy dead
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
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
            enemyHP -= 20;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
            enemyHP -= 20;
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
            enemyHP -= 5;
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
            enemyHP -= amount;
        }
    }

    public void ApplyDebuff(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        debuffTimer = Mathf.Max(debuffTimer, duration);
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
