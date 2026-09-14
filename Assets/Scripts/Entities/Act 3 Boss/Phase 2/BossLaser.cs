using UnityEngine;

public class BossLaser : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private FinalBossTwo finalBossTwo;
    [SerializeField] private float laserSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Direction vector from eye to player
        Vector2 direction = PhaseTwoManager.Instance.characterTransform.position - transform.position;
        //Angle from eye to player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Rotate aiming direction towards player
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Shoot laser after eye laser animation finishes
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position + (transform.right * 1.5f), Quaternion.AngleAxis(angle, Vector3.forward));
            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction.normalized * laserSpeed;
            //Play laser sound effect

            finalBossTwo.attacking = false;
            finalBossTwo.randomNum = Random.Range(0f, 1f);
            finalBossTwo.attackTime = 0f;
            gameObject.SetActive(false);
        }
    }
}
