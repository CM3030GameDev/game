using UnityEngine;

public class MissileAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameObject character;
    [SerializeField] private float missileSpeed;
    [SerializeField] private float rotateSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GameObject.FindWithTag("Character");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Destroy missile after 10 seconds
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        //Direction vector from missile to player
        Vector2 direction = character.transform.position - transform.position;

        //Angle difference between missile and player
        float angleDiff = Vector2.SignedAngle(transform.right, direction);

        //Homing missile rotation updated according to player position
        if(angleDiff != 0f)
        {
            transform.Rotate(Vector3.forward * angleDiff * Time.deltaTime);
        }
    }

    //private void FixedUpdate()
    //{
    //    //Missile keep flying at its forward direction
    //    rb.linearVelocity = transform.right * missileSpeed;
    //}
}
