using UnityEngine;

public class FinalBossTwo : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;
    private Vector2 normalizedChase;
    //Default material
    private Material defaultMaterial;
    //Boss max health
    public int bossHP;
    //Boss current health
    public int currentHP;
    //Boss current speed
    public float currentSpeed;
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
    //Check if barrier has been activated already (Can only activate once)
    public bool shielded;
    //Dashing state
    public bool dashing;
    //Character last position
    public Vector3 characterPos;
    //Beam direction (False is right, true is left)
    public bool beamDirection;
    //Random probability for boss pattern
    public float randomNum;

    [Header("References")]
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private DialogueData begin;
    //White material
    [SerializeField] private Material whiteMaterial;

    [Header("Attacks")]
    [SerializeField] private GameObject attacks;
    [SerializeField] private GameObject eyeLaser;
    [SerializeField] private GameObject beams;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject missileLauncher;
    [SerializeField] private GameObject dash;
    [SerializeField] private GameObject suctionEffect;
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private GameObject groundSmash;
    [SerializeField] private GameObject elementalProjectile;

    [Header("Values")]
    //Boss damage
    [SerializeField] private int damage;
    //Attack interval
    [SerializeField] private float intervals;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = bossHP;
        currentSpeed = characterStats.moveSpeed - 2;
        attacking = false;
        isAttacked = false;
        shielded = false;
        attackTime = 0f;
        randomNum = Random.Range(0f, 1f);
        defaultMaterial = sr.material;
        //Play boss dialogue
        DialogueManager.Instance.StartDialogue(begin);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Play boss BGM
        UIAudioManager.Instance.PlayBGM(0, true);
        //Play boss starting dialogue
        DialogueManager.Instance.StartDialogue(begin);
        //Set quest arrow target to be pointed towards Boss
        PhaseTwoManager.Instance.missionUI.SetArrowTarget(transform);
        //Initialise boss healthbar display
        PhaseTwoManager.Instance.slider.maxValue = bossHP;
        PhaseTwoManager.Instance.slider.value = bossHP;
        //Initialise boss healthbar text value
        PhaseTwoManager.Instance.tmp.text = $"{bossHP}/{bossHP}";
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTime < intervals)
        {
            attackTime += Time.deltaTime;
        }

        if (companionHitTimer > 0f) companionHitTimer -= Time.deltaTime;

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
            //Stop boss BGM
            UIAudioManager.Instance.StopBGM();
            //Pause everything in the scene except boss death animation
            Time.timeScale = 0f;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            //Boss death animation
            animator.SetTrigger("dead");
            //Play death sound effect
            UIAudioManager.Instance.PlaySFXOneShot(5, 0.5f);
            //Hide mission UI
            PhaseTwoManager.Instance.missionUI.Hide();
            //Stop spawning robot mobs
            MobManager.Instance.StopAllSpawnCoroutines();
            //Stop all attacks that are child gameobject of Boss
            attacks.SetActive(false);
            //Stop all other attacks that are not child gameobject of Boss
            for (int i = 0; i < PhaseTwoManager.Instance.attackList.Count; i++)
            {
                PhaseTwoManager.Instance.attackList[i].SetActive(false);
            }
            //Fade into darkness to transition to next scene
            PhaseTwoManager.Instance.transition.LoadSceneWithFade("Credits");
        }

        Vector2 chaseDirection = PhaseTwoManager.Instance.characterTransform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Boss is alive and currently not attacking
        if(!death)
        {
            //Activate barrier around boss when health goes below 50% of max health (Can only activate once)
            if (!shielded && currentHP < bossHP * 0.70)
            {
                Barrier();
            }

            if (!attacking)
            {
                //Flip sprite to face character
                if (PhaseTwoManager.Instance.characterTransform.position.x > transform.position.x)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }

                //Random attack between fixed intervals
                if (attackTime > intervals)
                {
                    attacking = true;

                    //If boss is within the center portion of the map, perform AoE attacks
                    if (Vector2.Distance(transform.position, new Vector3(25f, 5f, 0f)) <= 5f)
                    {
                        if (randomNum < 0.5f)
                        {
                            Beams();
                        }
                        else
                        {
                            Suction();
                        }
                    }
                    //If boss is not within center portion of the map
                    else
                    {
                        //If boss is far away from player current position
                        if (Vector2.Distance(PhaseTwoManager.Instance.characterTransform.position, transform.position) > 20f)
                        {
                            if (randomNum < 0.5f)
                            {
                                Dash();
                            }
                            else if (randomNum < 0.7f)
                            {
                                HomingMissile();
                            }
                            else if (randomNum < 0.8f)
                            {
                                GroundSmash();
                            }
                            else if(randomNum < 0.9f)
                            {
                                Laser();
                            }
                            else
                            {
                                ElementalProjectile();
                            }
                        }
                        //If boss is close to player current position
                        else
                        {
                            if (randomNum < 0.3f)
                            {
                                GroundSmash();
                            }
                            else if (randomNum < 0.6f)
                            {
                                ElementalProjectile();
                            }
                            else if (randomNum < 0.9f)
                            {
                                HomingMissile();
                            }
                            else
                            {
                                Laser();
                            }
                        }
                    }
                }
            }
            else
            {
                if(dashing)
                {
                    //Flip sprite to face character
                    if (PhaseTwoManager.Instance.characterTransform.position.x > transform.position.x)
                    {
                        sr.flipX = false;
                    }
                    else
                    {
                        sr.flipX = true;
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

    //Increases movement speed for a period of time
    private void Dash()
    {
        dashing = true;
        animator.SetBool("dash", true);
        dash.SetActive(true);
        //Increase boss current movement speed
        currentSpeed = characterStats.moveSpeed;
    }

    //Shoot laser at player position
    private void Laser()
    {
        eyeLaser.SetActive(true);
    }

    //Missile that chases player and detonate upon collision or automatically by itself after a period of time
    private void HomingMissile()
    {
        animator.SetBool("attack", true);
        missileLauncher.SetActive(true);
    }

    //Pulls player in before unleashing a shockwave nearby
    private void Suction()
    {
        animator.SetBool("attack", true);
        suctionEffect.SetActive(true);
        rangeIndicator.SetActive(true);
    }

    //Spawns ground impact attacks that chases player when smashing the ground
    private void GroundSmash()
    {
        animator.SetBool("smash", true);
        groundSmash.SetActive(true);
    }

    //Shoot elemental projectile attacks consecutively at player
    private void ElementalProjectile()
    {
        animator.SetBool("attack", true);
        elementalProjectile.SetActive(true);
    }

    //4 directional beam that rotates in a full circle (Random starting rotation, random direction)
    private void Beams()
    {
        animator.SetBool("attack", true);
        beams.SetActive(true);
    }

    //Barrier that reflects bullets for a period of time (Can only activate once)
    private void Barrier()
    {
        shielded = true;
        barrier.SetActive(true);
    }

    public void BossAttacked(int amount, float seconds)
    {
        // Every hit lands, the timer only drives the white flash, so shotgun pellets arriving together all count
        if (!death)
        {
            //Boss flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            currentHP -= amount;
            invulnTime = Mathf.Max(invulnTime, seconds);
            //Update boss health display
            PhaseTwoManager.Instance.slider.value = currentHP;
            //Update boss health text value
            PhaseTwoManager.Instance.tmp.text = $"{currentHP}/{bossHP}";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Character") && !PhaseTwoManager.Instance.character.isAttacked && !shielded)
        {
            PhaseTwoManager.Instance.character.CharacterAttacked(damage);
            PhaseTwoManager.Instance.character.GrantInvulnerability(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && companionHitTimer <= 0f && !shielded)
        {
            //Final boss flashes and take damage when attacked
            companionHitTimer = 0.3f;
            BossAttacked(50, 0.3f);
        }
    }

    // Companions use their own cooldown, since player shots now keep isAttacked on almost constantly
    private float companionHitTimer;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Flamethrower") && companionHitTimer <= 0f && !shielded)
        {
            //Final boss flashes and take damage when attacked
            companionHitTimer = 0.4f;
            BossAttacked(5, 0.4f);
        }
    }
}