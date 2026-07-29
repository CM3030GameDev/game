using UnityEngine;
using System.Collections;

public class Swing : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    private BoxCollider2D box;
    private Vector2 direction;
    private float attackTime;
    private float attackDuration;
    [SerializeField] private GameObject character;
    [SerializeField] private Mobs mobs;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private float detectionRange = 5f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        attackTime = 1f;
        attackDuration = 1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
                sr.flipY = true;
            }
            else
            {
                sr.flipY = false;
            }

            if (attackTime >= attackDuration)
            {
                //Swing attack animation
                animator.SetTrigger("attack");

                //attackTime = characterStats.attackSpeed;
                attackTime = 0.5f;
            }
        }
        //Does not attack if there is no nearby enemy
        else
        {

        }

        if (sr.sprite != null)
        {
            //Dynamically change hitbox according to swing animation
            box.size = sr.sprite.bounds.size;
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

        foreach (GameObject enemy in mobs.enemies)
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
}
