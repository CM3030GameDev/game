using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private GameObject soldier;
    private Character character;
    private float missileSpeed;
    private bool explode;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private float rotateSpeed;
    //Homing missile detonation time if no collision happens
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        soldier = GameObject.FindWithTag("Character");
        character = soldier.GetComponent<Character>();
    }

    private void OnEnable()
    {
        explode = false;
        missileSpeed = Random.Range(characterStats.moveSpeed - 4, characterStats.moveSpeed);
        StartCoroutine(ExplodeTime(lifeTime));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Direction vector from missile to player
        Vector2 direction = soldier.transform.position - transform.position;

        //Angle difference between missile and player
        float angleDiff = Vector2.SignedAngle(transform.right, direction);

        //Homing missile rotation updated according to player position
        if(angleDiff != 0f)
        {
            transform.Rotate(Vector3.forward * angleDiff * rotateSpeed * Time.deltaTime);
        }

        //Homing missile explosion animation ends
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Homing_Missile_Explosion") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Disable homing missile object
            gameObject.SetActive(false);
            //Return homing missile object back to object pool
            PhaseTwoManager.Instance.missiles.Enqueue(gameObject);
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
        //Collision with player
        if (collision.CompareTag("Character"))
        {
            //Player is attackable
            if(!character.isAttacked)
            {
                character.CharacterAttacked(30, 0.1f);
            }
            explode = true;
            //Explosion animation
            animator.SetTrigger("explode");
        }
        //Collision with walls
        else if(collision.CompareTag("Wall"))
        {
            explode = true;
            //Explosion animation
            animator.SetTrigger("explode");
        }
    }

    //Automatically explode missile if no collision happens within a period of time
    IEnumerator ExplodeTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        explode = true;
        //Explosion animation
        animator.SetTrigger("explode");
    }
}
