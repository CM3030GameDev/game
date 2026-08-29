using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private float missileSpeedMin;
    [SerializeField] private float missileSpeedMax;

    private void OnEnable()
    {
        //Direction vector from left missile launcher to player
        Vector2 leftDirection = new Vector2(character.transform.position.x, character.transform.position.y) - new Vector2(transform.position.x - 1.063f, transform.position.y);
        //Normalized direction from left missile launcher to player
        Vector2 leftNormalized = leftDirection.normalized;
        //Angle between left missile launcher and player
        float leftAngle = Mathf.Atan2(leftNormalized.y, leftNormalized.x) * Mathf.Rad2Deg;

        //Direction vector from right missile launcher to player
        Vector2 rightDirection = new Vector2(character.transform.position.x, character.transform.position.y) - new Vector2(transform.position.x + 1.063f, transform.position.y);
        //Normalized direction from right missile launcher to player
        Vector2 rightNormalized = rightDirection.normalized;
        //Angle between right missile launcher and player
        float rightAngle = Mathf.Atan2(rightNormalized.y, rightNormalized.x) * Mathf.Rad2Deg;

        //Random left missile speed
        float leftSpeed = Random.Range(missileSpeedMin, missileSpeedMax);
        //Random right missile speed
        float rightSpeed = Random.Range(missileSpeedMin, missileSpeedMax);

        //Left missile
        Instantiate(missilePrefab, new Vector2(transform.position.x - 1.063f, transform.position.y), Quaternion.AngleAxis(leftAngle, Vector3.forward));
        Rigidbody2D leftRb = missilePrefab.GetComponent<Rigidbody2D>();
        if (leftRb != null)
        {
            leftRb.linearVelocity = leftNormalized * leftSpeed;
        }

        //Right missile
        Instantiate(missilePrefab, new Vector2(transform.position.x + 1.063f, transform.position.y), Quaternion.AngleAxis(rightAngle, Vector3.forward));
        Rigidbody2D rightRb = missilePrefab.GetComponent<Rigidbody2D>();
        if (rightRb != null)
        {
            rightRb.linearVelocity = rightNormalized * rightSpeed;
        }

        finalBoss.randomNum = Random.Range(0f, 1f);
        finalBoss.attackTime = 0f;
        finalBoss.attacking = false;
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
