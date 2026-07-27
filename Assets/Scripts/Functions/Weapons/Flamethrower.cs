using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 direction;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject character;
    [SerializeField] private EnemyWave enemyWave;
    [SerializeField] private float detectionRange = 10f;

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
        }
        //Does not attack if there is no nearby enemy
        else
        {
            fire.SetActive(false);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = detectionRange;

        foreach (GameObject enemy in enemyWave.enemies)
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
