using System.Collections;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    private int missileCount;
    private bool shooting;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private FinalBossTwo finalBossTwo;
    [SerializeField] private Animator animator;
    [SerializeField] private float interval;

    private void OnEnable()
    {
        shooting = false;
        //Random number of homing missiles ranging from 2 to 10 (In multiples of 2 since missile launcher can fire 2 homing missiles simultaneously)
        missileCount = Random.Range(1, 6) * 2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(missileCount > 0)
        {
            if (!shooting)
            {
                StartCoroutine(MissileTime(interval));
            }
        }
        else
        {
            //End boss attack animation
            animator.SetBool("attack", false);
            //Stop homing missile attack
            gameObject.SetActive(false);
        }
    }

    //Interval between firing sets of homing missiles (2 homing missiles fired at a time)
    IEnumerator MissileTime(float seconds)
    {
        shooting = true;
        //Decrease missile count by 2
        missileCount -= 2;

        //Direction vector from left missile launcher to player
        Vector2 leftDirection = (Vector2)PhaseTwoManager.Instance.characterTransform.position - new Vector2(transform.position.x - 1.063f, transform.position.y);
        //Normalized direction from left missile launcher to player
        Vector2 leftNormalized = leftDirection.normalized;
        //Angle between left missile launcher and player
        float leftAngle = Mathf.Atan2(leftNormalized.y, leftNormalized.x) * Mathf.Rad2Deg;

        //Direction vector from right missile launcher to player
        Vector2 rightDirection = (Vector2)PhaseTwoManager.Instance.characterTransform.position - new Vector2(transform.position.x + 1.063f, transform.position.y);
        //Normalized direction from right missile launcher to player
        Vector2 rightNormalized = rightDirection.normalized;
        //Angle between right missile launcher and player
        float rightAngle = Mathf.Atan2(rightNormalized.y, rightNormalized.x) * Mathf.Rad2Deg;

        //Left missile
        GameObject leftMissile = PhaseTwoManager.Instance.missiles.Dequeue();
        //Fire left missile from left missile launcher position
        leftMissile.transform.position = new Vector3(transform.position.x - 1.063f, transform.position.y, 0f);
        //Angle it towards player current position
        leftMissile.transform.rotation = Quaternion.Euler(0f, 0f, leftAngle);
        //Start shooting left missile
        leftMissile.SetActive(true);
        //Random left missile speed
        float leftSpeed = Random.Range(characterStats.moveSpeed - 4, characterStats.moveSpeed);
        Rigidbody2D leftRb = leftMissile.GetComponent<Rigidbody2D>();
        leftRb.linearVelocity = transform.right * leftSpeed;

        //Right missile
        GameObject rightMissile = PhaseTwoManager.Instance.missiles.Dequeue();
        //Fire right missile from right missile launcher position
        rightMissile.transform.position = new Vector2(transform.position.x + 1.063f, transform.position.y);
        //Angle it towards player current position
        rightMissile.transform.rotation = Quaternion.Euler(0f, 0f, rightAngle);
        //Start shooting right missile
        rightMissile.SetActive(true);
        //Random right missile speed
        float rightSpeed = Random.Range(characterStats.moveSpeed - 4, characterStats.moveSpeed);
        Rigidbody2D rightRb = rightMissile.GetComponent<Rigidbody2D>();
        rightRb.linearVelocity = transform.right * rightSpeed;

        yield return new WaitForSeconds(seconds);
        shooting = false;
    }
}
