using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private SpriteRenderer swingSprite;
    private Animator animator;
    private BoxCollider2D box;
    private Vector2 direction;
    private float attackTime;
    private float attackDuration;
    [SerializeField] private SpriteRenderer companionSprite;
    [SerializeField] private GameObject character;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private int damage = 6;
    [SerializeField] private float damageCD = 0.2f;

    private List<GameObject> allEnemies;

    private void Awake()
    {
        swingSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        attackTime = 1f;
        attackDuration = 1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allEnemies = MobManager.Instance.GetAllPooledEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        //Duration until next attack
        attackTime += Time.deltaTime;

        //Get the nearest enemy
        GameObject target = FindNearestEnemy();

        //Check if nearby enemy exist
        if (target != null)
        {
            //Vector direction between nearest enemy and swordsman
            direction = target.transform.position - transform.position;

            //Convert angle from radian to degree
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //Current aim
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Flip sprite according to aim position
            if (target.transform.position.x < transform.position.x)
            {
                swingSprite.flipY = true;
            }
            else
            {
                swingSprite.flipY = false;
            }

            //Flip companion sprite according to nearest enemy position
            if (target.transform.position.x > transform.position.x)
            {
                companionSprite.flipX = true;
            }
            else
            {
                companionSprite.flipX = false;
            }

            if (attackTime >= attackDuration)
            {
                //Swing attack
                animator.SetTrigger("attack");

                attackTime = characterStats.attackSpeed;
            }
        }
        //Does not attack if there is no nearby enemy
        else
        {
            //Flip companion sprite according to character position
            if (character.transform.position.x > transform.position.x)
            {
                companionSprite.flipX = true;
            }
            else
            {
                companionSprite.flipX = false;
            }
        }

        if (swingSprite.sprite != null)
        {
            //Dynamically change hitbox according to swing animation
            box.size = swingSprite.sprite.bounds.size;
        }
        else
        {
            //Set collider size to be zero when not in swing animation
            box.size = Vector2.zero;
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = detectionRange;

        foreach (GameObject enemy in allEnemies)
        {
            if(!enemy.activeInHierarchy)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IDamageable target = collision.GetComponent<IDamageable>();
            target?.TakeDamage(damage, damageCD);
        }
    }
}
