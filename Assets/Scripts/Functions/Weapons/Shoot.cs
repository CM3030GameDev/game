using UnityEngine;

public class Shoot : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private float reloadTime;
    private float reloadDuration;
    //Toggleable auto aim and attack
    private bool autoAttack;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private float bulletSpeed = 50f;

    private void Awake()
    {
        mainCamera = Camera.main;
        autoAttack = false;
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
        //Get mouse position in screen pixels
        Vector3 screenPosition = Input.mousePosition;

        //Convert screen pixels to 3D world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        //Convert 3D world coordinate to 2D 
        worldPosition.z = 0f;

        //Direction vector from player to mouse
        Vector2 direction = worldPosition - transform.position;

        //Normalize direction vector from player to mouse
        Vector2 normalizedDirection = direction.normalized;

        //Convert angle from radian to degree
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Duration until next attack
        reloadTime += Time.deltaTime;

        //Auto aim and attack
        if (Input.GetMouseButtonDown(1))
        {
            autoAttack = !autoAttack;
        }

        //Prevent player from attacking again until duration is over
        if (reloadTime > reloadDuration)
        {
            if (Input.GetMouseButton(0) || autoAttack)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
                rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = normalizedDirection * bulletSpeed;
                }
                reloadTime = characterStats.attackSpeed;
            }
        }
    }
}
