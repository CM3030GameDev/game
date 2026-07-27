using UnityEngine;

public class Pistol : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float reloadTime;
    private float reloadDuration;
    public float angle;
    private Vector2 direction;
    //Toggleable auto aim
    public bool autoAim;
    [SerializeField] private CharacterStats cs;
    [SerializeField] private Sprite pistol;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private EnemyWave enemyWave;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float detectionRange = 20f;

    private void Awake()
    {
        mainCamera = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        autoAim = false;
        reloadTime = 1f;
        reloadDuration = 1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Duration until next attack
        reloadTime += Time.deltaTime;

        //Toggle between auto and manual aiming
        if (Input.GetMouseButtonDown(1))
        {
            autoAim = !autoAim;
        }

        //Auto aim
        if (autoAim)
        {
            //Get the nearest enemy
            GameObject target = FindNearestEnemy();

            //Auto aim and shoot if nearby enemy exist
            if (target != null)
            {
                AutoAim(target);
                //Prevent player from attacking again until duration is over
                if (reloadTime >= reloadDuration)
                {
                    Shoot(angle, direction);
                }
            }
        }
        //Manual aim
        else
        {
            ManualAim();
            //Manual shoot
            if(Input.GetMouseButton(0))
            {
                //Prevent player from attacking again until duration is over
                if (reloadTime >= reloadDuration)
                {
                    Shoot(angle, direction);
                }
            }
        }
    }

    private void ManualAim()
    {
        //Aim at mouse cursor position
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //Normalize direction vector from player to mouse
        direction = ((Vector2)mousePos - (Vector2)transform.position).normalized;

        //Convert angle from radian to degree
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Current aim
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Flip sprite according to aim position
        if (mousePos.x < transform.position.x)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }
    }

    private void AutoAim(GameObject enemy)
    {
        //Normalized vector direction between nearest enemy and character
        direction = ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

        //Convert angle from radian to degree
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Current aim
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Flip sprite according to aim position
        if (enemy.transform.position.x < transform.position.x)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }
    }

    private void Shoot(float bulletRotation, Vector2 bulletDirection)
    {
        //Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.121f, 0), Quaternion.AngleAxis(bulletRotation, Vector3.forward));
        rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = bulletDirection * bulletSpeed;
            //Refresh bullet reload timer
            reloadTime = characterStats.attackSpeed;
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
