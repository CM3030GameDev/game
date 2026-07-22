using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private float projectileSpeed = 50f;
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private WeaponAttack weaponAttack;
    private Camera mainCamera;
    private Rigidbody2D projectileRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
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

        //Countdown for next attack
        companionSystem.characterAS -= Time.deltaTime;

        //Prevent player from attacking again until countdown is finished
        if (companionSystem.characterAS < 0)
        {
            if (Input.GetMouseButton(0) || weaponAttack.autoAttack)
            {
                GameObject projectiles = Instantiate(pistolBullet, transform.position, Quaternion.Euler(0, 0, angle));
                projectileRB = projectiles.GetComponent<Rigidbody2D>();
                projectileRB.AddForce(normalizedDirection * projectileSpeed, ForceMode2D.Impulse);
                Destroy(projectiles, 2f);
                companionSystem.characterAS = 1f;
            }
        }
    }
}
