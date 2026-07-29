using UnityEngine;

public class Companion : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private float distance;
    private Vector2 normalizedFollow;
    [SerializeField] private GameObject character;
    [SerializeField] private float followRange = 2f;
    [SerializeField] private float followSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(character.transform.position, transform.position);

        Vector2 followDirection = character.transform.position - transform.position;
        normalizedFollow = followDirection.normalized;

        //Flip sprite according to aim position
        if (character.transform.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        //Follow if out of range of character
        if (distance > followRange)
        {
            animator.SetBool("move", true);
            rb.linearVelocity = normalizedFollow * followSpeed;
        }
        //Stop following if within range of character
        else
        {
            animator.SetBool("move", false);
            rb.linearVelocity = Vector2.zero;
        }
    }
}
