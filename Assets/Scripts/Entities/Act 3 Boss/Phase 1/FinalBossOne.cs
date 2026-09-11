using UnityEngine;

public class FinalBossOne : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    //Boss max health
    public int bossHP;
    //Boss current health
    public int currentHP;
    //Boss current speed
    public float currentSpeed;
    //Chase direction
    private Vector2 normalizedChase;
    //Attacked state
    private bool isAttacked;
    //Invulnerable timer
    private float invulnTime;
    //Attack timer
    public float attackTime;
    //Random attack probability
    public float randomNum;
    //Attacking state
    public bool attacking;
    //Dash state
    public bool dashing;
    //Check if flames has been activated already (Can only activate once)
    private bool flamed;
    //Check if smokescreen has been used (Can only use once)
    private bool smoked;
    //Death state
    private bool death;
    //Default material
    private Material defaultMaterial;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private GameObject attacks;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject flames;
    [SerializeField] private GameObject fireCannon;
    [SerializeField] private GameObject missileBarrage;
    [SerializeField] private GameObject smokeScreens;
    //White material
    [SerializeField] private Material whiteMaterial;
    //Attack interval
    [SerializeField] private float intervals;
    [SerializeField] private int damage;
    [SerializeField] private DialogueData begin;
    [SerializeField] private DialogueData end;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultMaterial = sr.material;
        currentHP = bossHP;
        currentSpeed = characterStats.moveSpeed - 2;
        isAttacked = false;
        attacking = false;
        flamed = false;
        smoked = false;
        death = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Play Villain dialogue
        DialogueManager.Instance.StartDialogue(begin);
        //Initialise boss healthbar display
        PhaseOneManager.Instance.slider.maxValue = bossHP;
        PhaseOneManager.Instance.slider.value = bossHP;
        //Initialise boss healthbar text value
        PhaseOneManager.Instance.tmp.text = $"{bossHP}/{bossHP}";
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
        if (currentHP <= 0 && !death)
        {
            death = true;
            //Stop all attacks currently
            attacks.SetActive(false);
            //Boss death animation
            animator.SetTrigger("dead");
            death = true;
            //Play Villain dialogue
            DialogueManager.Instance.StartDialogue(end);
        }

        Vector2 chaseDirection = PhaseOneManager.Instance.characterTransform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Boss is alive and currently not attacking
        if (!death)
        {
            //Flip sprite to face character
            if (PhaseOneManager.Instance.characterTransform.position.x > transform.position.x)
            {
                sr.flipX = false;
            }
            else
            {
                sr.flipX = true;
            }

            //Activate flames around boss when health is below 70% of max health (Can be used once only)
            if (!flamed && currentHP < bossHP * 0.70)
            {
                Flames();
            }

            if (!attacking)
            {
                //Random attack between fixed intervals
                if (attackTime > intervals)
                {
                    attacking = true;

                    //Use smokescreen when boss health goes below 50% of max health (Can be used once only)
                    if (!smoked && currentHP < bossHP * 0.50)
                    {
                        SmokeScreen();
                    }

                    //Player is far away from boss
                    if (Vector2.Distance(PhaseOneManager.Instance.characterTransform.position, transform.position) > 20f)
                    {
                        if (randomNum < 0.5f)
                        {
                            Dash();
                        }
                        else
                        {
                            Missile();
                        }
                    }
                    //Player is close to the boss
                    else
                    {
                        attacking = true;

                        if (randomNum < 0.7f)
                        {
                            FireCannon();
                        }
                        else
                        {
                            Missile();
                        }
                    }
                }
            }
        }
        //Boss died
        else
        {
            //End of phase one dialogue
            DialogueManager.Instance.StartDialogue(end);
        }
    }

    private void FixedUpdate()
    {
        //Boss died
        if (death)
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
                rb.linearVelocity = normalizedChase * currentSpeed;
            }
            else
            {
                //Dash towards player at high speed
                if (dashing)
                {
                    rb.linearVelocity = normalizedChase * currentSpeed;
                }
                //Stationary when attacking
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    //Dash towards player at a high speed
    private void Dash()
    {
        dashing = true;
        //Start dashing animation
        animator.SetBool("dash", true);
        dash.SetActive(true);
        //Increase boss current movement speed
        currentSpeed = characterStats.moveSpeed;
    }

    //Flame surrounds boss for a period of time
    private void Flames()
    {
        flamed = true;
        flames.SetActive(true);
    }

    //Create fog by emitting smoke (Can only use once)
    private void SmokeScreen()
    {
        smoked = true;
        //Start attack animation
        animator.SetBool("attack", true);
        //Start smokescreen animation
        smokeScreens.SetActive(true);
        //Fog slowly appear
        PhaseOneManager.Instance.fogs.SetActive(true);
    }

    //Spray fire attack consecutively at player at close range
    private void FireCannon()
    {
        //Start attack animation
        animator.SetBool("attack", true);
        fireCannon.SetActive(true);
    }

    //Fire missile into the sky, it will attack random position at the current screen position after a period of time
    private void Missile()
    {
        animator.SetBool("attack", true);
        missileBarrage.SetActive(true);
    }

    public void BossAttacked(int amount, float seconds)
    {
        if (!isAttacked && !death)
        {
            //Boss flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            currentHP -= amount;
            invulnTime = seconds;
            //Update boss health display
            PhaseOneManager.Instance.slider.value = currentHP;
            //Update boss health text value
            PhaseOneManager.Instance.tmp.text = $"{currentHP}/{bossHP}";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && !isAttacked)
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
