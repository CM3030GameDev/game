using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 direction;
    [SerializeField] private SpriteRenderer companionSprite;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject character;
    [SerializeField] private Mobs mobs;
    [SerializeField] private float detectionRange = 6f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Get the nearest enemy
        GameObject target = FindNearestEnemy();

        //Automatically attack if nearby enemy exist
        if (target != null)
        {
            fire.SetActive(true);

            //Vector direction between nearest enemy and mercenary
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

            //Flip companion sprite according to nearest enemy position
            if (target.transform.position.x > transform.position.x)
            {
                companionSprite.flipX = true;
            }
            else
            {
                companionSprite.flipX = false;
            }
        }
        //Does not attack if there is no nearby enemy
        else
        {
            fire.SetActive(false);

            //Flip companion sprite according to character position
            if (character.transform.position.x > transform.position.x)
            {
                companionSprite.flipX = true;
                transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
                sr.flipY = false;
            }
            else
            {
                companionSprite.flipX = false;
                transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
                sr.flipY = true;
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = detectionRange;

        foreach (GameObject enemy in mobs.enemies)
        {
            if (!enemy.activeInHierarchy)
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
