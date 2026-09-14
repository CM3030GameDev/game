using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Vector2 movement;
    private float invulnTimer;
    private float flickerTimer;
    private float regenAccumulator;
    public bool confused;
    public bool IsInvulnerable => invulnTimer > 0f;
    public Vector2 MoveInput => movement;
    //Attacked state
    public bool isAttacked;
    [Tooltip("Seconds per on/off step while blinking after a hit.")]
    [SerializeField] private float flickerInterval = 0.08f;
    [Tooltip("Index into UIAudioManager's Sound Effects list, played when the player is hit. " +
             "-1 plays nothing.")]
    [SerializeField] private int hurtSfx = -1;
    [Range(0f, 1f)][SerializeField] private float hurtSfxVolume = 0.7f;
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private CharacterStats cs;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        isAttacked = false;
        confused = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        Sprite();
        Regenerate();

        //Character cannot be attacked
        if (invulnTimer > 0f)
        {
            invulnTimer -= Time.deltaTime;
        }
        //Character can be attacked again
        else
        {
            isAttacked = false;
            animator.SetBool("attacked", false);
        }

        Flicker();
    }

    private void FixedUpdate()
    {
        //Character alive
        if(cs.health > 0)
        {
            //Normal status
            if(!confused)
            {
                rb.linearVelocity = movement * cs.moveSpeed;
            }
            //Confused status effect
            else
            {
                rb.linearVelocity = movement * -1 * cs.moveSpeed;
            }
        }
        //Character dead
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Movement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        //Normalize diagonal direction vector to ensure it has magnitude of 1
        movement = new Vector2(inputX, inputY).normalized;

        if (inputX != 0 || inputY != 0)
        {
            animator.SetBool("move", true);
        }
        else
        {
            animator.SetBool("move", false);
        }
    }

    private void Sprite()
    {
        // Flips the player's sprite based on where they are looking
        sr.flipX = playerAim.AimDirection.x > 0f;
    }

    private void Regenerate()
    {
        // Not while dead, or regen could revive the player before PlayerRespawn notices
        if (cs.health <= 0 || cs.healthRegen <= 0f || cs.health >= cs.maxHealth) return;

        // Accumulate fractional regen so low rates (e.g. 1/sec) still add up over time
        regenAccumulator += cs.healthRegen * Time.deltaTime;
        int whole = Mathf.FloorToInt(regenAccumulator);
        if (whole > 0)
        {
            cs.health = Mathf.Min(cs.maxHealth, cs.health + whole);
            regenAccumulator -= whole;
        }
    }

    // Starts the invulnerability window itself, so no caller has to remember to.
    public void CharacterAttacked(int amount, float invulnerability = 0.5f)
    {
        if (IsInvulnerable || cs.health <= 0) return;

        // Play attacked animation of character
        animator.SetBool("attacked", true);
        cs.health -= amount;
        GrantInvulnerability(invulnerability);
        flickerTimer = invulnerability;

        // Inside the IsInvulnerable guard above, so repeated contact does not machine-gun the clip.
        UIAudioManager.Sfx(hurtSfx, hurtSfxVolume);
    }

    // Blinks on its own timer rather than on invulnTimer, because GrantInvulnerability is also
    // used by the Guard skill, which lasts seconds and has its own shield visual.
    private void Flicker()
    {
        if (flickerTimer <= 0f) return;

        flickerTimer -= Time.deltaTime;
        sr.enabled = flickerTimer <= 0f ||
                     Mathf.Repeat(flickerTimer, flickerInterval * 2f) > flickerInterval;
    }

    public void GrantInvulnerability(float duration)
    {
        invulnTimer = Mathf.Max(invulnTimer, duration);
    }
}
