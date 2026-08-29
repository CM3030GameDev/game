using UnityEngine;

public class MissileAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private GameObject soldier;
    private Character character;
    private float missileSpeed;
    private float timer;
    private bool explode;
    [SerializeField] private float missileSpeedMin;
    [SerializeField] private float missileSpeedMax;
    [SerializeField] private float rotateSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        soldier = GameObject.FindWithTag("Character");
        character = soldier.GetComponent<Character>();
        //Random missile speed
        missileSpeed = Random.Range(missileSpeedMin, missileSpeedMax);
        timer = 0f;
        explode = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Destroy missile after 10 seconds
        if(timer > 10f)
        {
            explode = true;
            //Explosion animation
            animator.SetTrigger("explode");
        }
        //Continue counting
        else
        {
            timer += Time.deltaTime;
        }

        //Direction vector from missile to player
        Vector2 direction = soldier.transform.position - transform.position;

        //Angle difference between missile and player
        float angleDiff = Vector2.SignedAngle(transform.right, direction);

        //Homing missile rotation updated according to player position
        if(angleDiff != 0f)
        {
            transform.Rotate(Vector3.forward * angleDiff * rotateSpeed * Time.deltaTime);
        }
    }   

    private void FixedUpdate()
    {
        if(explode)
        {
            //Explode missile at current spot
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            //Missile keep flying at its forward direction
            rb.linearVelocity = transform.right * missileSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            //Player is attackable
            if(!character.isAttacked)
            {
                character.CharacterAttacked(30);
                character.GrantInvulnerability(0.1f);
            }
            explode = true;
            //Explosion animation
            animator.SetTrigger("explode");
        }
    }
}
