using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    private bool missileAttack;
    private float missileInterval;
    private float leftAngle;
    private float rightAngle;
    private Vector2 direction;
    private Vector2 leftDirection;
    private Vector2 rightDirection;
    [SerializeField] private GameObject character;
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float missileSpeedMin;
    [SerializeField] private float missileSpeedMax;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        missileInterval = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(missileInterval > 0f)
        {
            missileInterval -= Time.deltaTime;
        }

        //If player is far away from boss, use homing missile attack
        if(Vector2.Distance(character.transform.position, transform.position) > 30f && missileInterval <= 0f & !finalBoss.attacking)
        {
            finalBoss.attacking = true;
            missileInterval = 10f;

            //Direction vector from left missile launcher to player
            direction = new Vector2(character.transform.position.x, character.transform.position.y) - new Vector2(transform.position.x - 1.063f, transform.position.y);
            //Normalized direction from left missile launcher to player
            leftDirection = direction.normalized;
            //Angle between left missile launcher and player
            float leftAngle = Mathf.Atan2(leftDirection.y, leftDirection.x) * Mathf.Rad2Deg;

            //Direction vector from right missile launcher to player
            direction = new Vector2(character.transform.position.x, character.transform.position.y) - new Vector2(transform.position.x + 1.063f, transform.position.y);
            //Normalized direction from right missile launcher to player
            rightDirection = direction.normalized;
            //Angle between right missile launcher and player
            float rightAngle = Mathf.Atan2(rightDirection.y, rightDirection.x) * Mathf.Rad2Deg;
        }

        if (missileAttack)
        {
            missileAttack = false;

            //Random left missile speed
            float leftSpeed = Random.Range(missileSpeedMin, missileSpeedMax);
            //Random right missile speed
            float rightSpeed = Random.Range(missileSpeedMin, missileSpeedMax);

            //Left missile
            Instantiate(missilePrefab, new Vector2(transform.position.x - 1.063f, transform.position.y), Quaternion.AngleAxis(leftAngle, Vector3.forward));
            Rigidbody2D leftRb = missilePrefab.GetComponent<Rigidbody2D>();
            if (leftRb != null)
            {
                leftRb.linearVelocity = leftDirection * leftSpeed;
            }

            //Right missile
            Instantiate(missilePrefab, new Vector2(transform.position.x + 1.063f, transform.position.y), Quaternion.AngleAxis(rightAngle, Vector3.forward));
            Rigidbody2D rightRb = missilePrefab.GetComponent<Rigidbody2D>();
            if (rightRb != null)
            {
                rightRb.linearVelocity = rightDirection * rightSpeed;
            }
        }
    }
}
