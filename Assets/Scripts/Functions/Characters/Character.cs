using UnityEngine;

public class Character : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Vector2 movement;
    //Attacked state
    private bool isAttacked;
    //Attacked state timer
    private float attackedTime;
    //Attacked state duration
    private float attackedDuration;
    [SerializeField] private CharacterStats cs;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        isAttacked = false;
        attackedTime = 1f;
        attackedDuration = 0.1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Movement();
        Aim();

        attackedTime += Time.deltaTime;

        //Character can be attacked again
        if (attackedTime > attackedDuration)
        {
            isAttacked = false;
            animator.SetBool("attacked", false);
        }
    }

    private void FixedUpdate()
    {
        //Character alive
        if(cs.health > 0)
        {
            rb.linearVelocity = movement * cs.moveSpeed;
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

    private void Aim()
    {
        //Get mouse position in screen pixels
        Vector3 screenPosition = Input.mousePosition;

        //Convert screen pixels to 3D world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        //Flip sprite according to aim position
        if(worldPosition.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    public void CharacterAttacked(int amount)
    {
        if (!isAttacked)
        {
            //Attacked animation of character
            animator.SetBool("attacked", true);
            isAttacked = true;
            attackedTime = 0f;
            cs.health -= amount;
        }
    }
}