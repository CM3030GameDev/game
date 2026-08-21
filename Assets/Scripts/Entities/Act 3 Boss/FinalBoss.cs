using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private GameObject character;
    //Vector direction towards player
    private Vector2 chaseDirection;
    //Normalized vector direction towards player
    private Vector2 normalizedChase;
    //Attacked state
    private bool isAttacked;
    //Attacked state timer
    private float attackedTime;
    //Death state
    private bool death;
    //Attacking state
    public bool attacking;
    //Beam direction (False is right, true is left)
    public bool beamDirection;
    //Default material
    private Material defaultMaterial;
    //Boss HP
    [SerializeField] private int bossHP;
    //White material
    [SerializeField] private Material whiteMaterial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        attacking = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        character = GameObject.FindGameObjectsWithTag("Character")[0];
        defaultMaterial = sr.material;
    }

    // Update is called once per frame
    void Update()
    {
        chaseDirection = character.transform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Flip sprite to face character
        if (character.transform.position.x > transform.position.x && !attacking)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }

        //Boss cannot be attacked
        if (attackedTime > 0)
        {
            attackedTime -= Time.deltaTime;
        }
        //Boss can be attacked
        else
        {
            //Change back to default material
            sr.material = defaultMaterial;
            isAttacked = false;
        }

        //Boss dead
        if (bossHP <= 0 && !death)
        {
            animator.SetTrigger("dead");
            death = true;
        }

        //For testing purpose only
        if(Input.GetKeyDown(KeyCode.F))
        {
            Beam();
        }

        //For testing attack purpose only
        if (Input.GetKeyDown(KeyCode.G))
        {
            BossAttacked(0.3f, 50);
        }
    }

    private void Beam()
    {
        if(!attacking)
        {
            attacking = true;
            //Beam starts from right
            if (character.transform.position.x > transform.position.x)
            {
                beamDirection = false;
            }
            //Beam starts from left
            else
            {
                beamDirection = true;
            }
            animator.SetTrigger("beam_start");
        }
    }

    public void BossAttacked(float duration, int amount)
    {
        if (!isAttacked)
        {
            // Final boss flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            bossHP -= amount;
            attackedTime = duration;
        }
    }
}
