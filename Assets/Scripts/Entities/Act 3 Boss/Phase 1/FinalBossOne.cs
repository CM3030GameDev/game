using UnityEngine;

public class FinalBossOne : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    //Default material
    private Material defaultMaterial;
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
    //Check if smokescreen has been used (Can only use once)
    private bool smoked;
    //Death state
    public bool death;

    [Header("References")]
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private DialogueData begin;
    [SerializeField] private DialogueData end;
    //White material
    [SerializeField] private Material whiteMaterial;

    [Header("Attacks")]
    [SerializeField] private GameObject attacks;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject flames;
    [SerializeField] private GameObject fireCannon;
    [SerializeField] private GameObject missileBarrage;
    [SerializeField] private GameObject smokeScreens;

    [Header("Values")]
    //Attack interval
    [SerializeField] private float intervals;
    [SerializeField] private int damage;

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
        smoked = false;
        death = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Play Villain starting dialogue
        DialogueManager.Instance.StartDialogue(begin);
        //Set quest arrow target to be pointed towards Villain
        PhaseOneManager.Instance.missionUI.SetArrowTarget(transform);
        //Initialise villain healthbar display
        PhaseOneManager.Instance.slider.maxValue = bossHP;
        PhaseOneManager.Instance.slider.value = bossHP;
        //Initialise villain healthbar text value
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
            //Boss death animation
            animator.SetTrigger("dead");
            //Play death sound effect
            UIAudioManager.Instance.PlaySFXOneShot(5, 1f);
            //Hide mission UI
            PhaseOneManager.Instance.missionUI.Hide();
            //Play Villain death dialogue
            DialogueManager.Instance.StartDialogue(end);
            //Stop spawning robot mobs
            MobManager.Instance.StopAllSpawnCoroutines();
            //Point quest arrow towards door position
            PhaseOneManager.Instance.ArrowPointer();
            //Enable door collider for player to transition to Act 3 phase 2 scene when within range of door
            PhaseOneManager.Instance.doorCollider.enabled = true;
            //Stop all attacks that are child gameobject of Villain
            attacks.SetActive(false);
            //Stop all other attacks that are not child gameobject of Villain
            for(int i = 0; i < PhaseOneManager.Instance.attackList.Count; i++)
            {
                PhaseOneManager.Instance.attackList[i].SetActive(false);
            }
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

                        if (randomNum < 0.6f)
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

    //Create fog by emitting smoke (Can only use once)
    private void SmokeScreen()
    {
        smoked = true;
        //Start attack animation
        animator.SetBool("attack", true);
        //Start smokescreen animation
        smokeScreens.SetActive(true);
        //Play smoke sound effect
        PhaseOneManager.Instance.PlayAudio();
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
            //Villain flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            currentHP -= amount;
            invulnTime = seconds;
            //Update villain health display
            PhaseOneManager.Instance.slider.value = currentHP;
            //Update villain health text value
            PhaseOneManager.Instance.tmp.text = $"{currentHP}/{bossHP}";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Only able to attack player when alive
        if (collision.gameObject.CompareTag("Character") && !death)
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
