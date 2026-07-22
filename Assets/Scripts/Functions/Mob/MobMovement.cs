using UnityEngine;

public class MobMovement : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private Rigidbody2D mobRB;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator mobSprite;
    [SerializeField] private RuntimeAnimatorController blueMob;
    [SerializeField] private RuntimeAnimatorController redMob;
    [SerializeField] private RuntimeAnimatorController greenMob;
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private EnemySystem enemySystem;
    //Attacked state duration
    [SerializeField] private float attackedDuration = -0.1f;
    //Attacked state
    private bool isAttacked;
    public int enemyHP;
    private Vector2 chaseDirection;
    private Vector2 normalizedChase;
    private bool isBlue;
    private bool isRed;
    private bool isGreen;
    private bool death;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        death = false;
    }

    private void OnEnable()
    {
        isBlue = false;
        isRed = false;
        isGreen = false;
        isAttacked = false;
        death = false;
        float randomNum = Random.Range(0f, 3f);

        if(randomNum < 1f)
        {
            isBlue = true;
        }
        else if(randomNum < 2f)
        {
            isRed = true;
        }
        else if(randomNum < 3f)
        {
            isGreen = true;
        }

        if (isBlue)
        {
            mobSprite.runtimeAnimatorController = blueMob;
            enemyHP = 100;
        }
        else if (isRed)
        {
            mobSprite.runtimeAnimatorController = redMob;
            enemyHP = 50;
        }
        else if (isGreen)
        {
            mobSprite.runtimeAnimatorController = greenMob;
            enemyHP = 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Dynamically change hitbox according to each sprite in animation
        hitBox.size = spriteRenderer.sprite.bounds.size;

        chaseDirection = GameObject.FindGameObjectsWithTag("Character")[0].transform.position - transform.position;
        normalizedChase = chaseDirection.normalized;

        //Convert angle from radian to degree
        float angle = Mathf.Atan2(chaseDirection.y, chaseDirection.x) * Mathf.Rad2Deg;

        //Offset angle
        angle += 45f;

        //Convert negative angle to positive angle
        if (angle < 0)
        {
            angle += 360;
        }

        //Determine which direction enemy face
        int direction = Mathf.FloorToInt(angle / 90);

        //Face right
        if (direction == 0)
        {
            mobSprite.SetFloat("direction", 0);
        }
        //Face up
        else if (direction == 1)
        {
            mobSprite.SetFloat("direction", 1);
        }
        //Face left
        else if (direction == 2)
        {
            mobSprite.SetFloat("direction", 2);
        }
        //Face down
        else if (direction == 3)
        {
            mobSprite.SetFloat("direction", 3);
        }

        attackedDuration -= Time.deltaTime;

        if (attackedDuration < 0)
        {
            isAttacked = false;
            mobSprite.SetBool("attacked", false);
        }

        if (enemyHP <= 0 && !death)
        {
            //Death animation of enemy
            mobSprite.SetTrigger("dead");
            death = true;
        }
    }

    private void FixedUpdate()
    {
        if (enemyHP > 0)
        {
            mobRB.linearVelocity = normalizedChase * chaseSpeed;
        }
        //Stop moving if enemy is dead
        else if(enemyHP <= 0)
        {
            mobRB.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isAttacked)
        {
            //Attacked animation of enemy
            mobSprite.SetBool("attacked", true);
            isAttacked = true;
            enemyHP -= 20;
            attackedDuration = 0.05f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !isAttacked)
        {
            //Attacked animation of enemy
            mobSprite.SetBool("attacked", true);
            isAttacked = true;
            enemyHP -= 20;
            attackedDuration = 0.05f;
        }

        if (collision.gameObject.CompareTag("Character"))
        {
            if(companionSystem.playerCharacter == "soldier")
            {
                companionSystem.soldierHealth -= 10;
            }
            if (companionSystem.playerCharacter == "mercenary")
            {
                companionSystem.mercenaryHealth -= 10;
            }
            if (companionSystem.playerCharacter == "swordsman")
            {
                companionSystem.swordsmanHealth -= 10;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flamethrower") && !isAttacked)
        {
            //Attacked animation of enemy
            mobSprite.SetBool("attacked", true);
            isAttacked = true;
            enemyHP -= 5;
            attackedDuration = 0.4f;
        }
    }
}
