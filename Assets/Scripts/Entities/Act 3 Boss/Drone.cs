using UnityEngine;

public class Drone : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    public bool attacking;
    private bool isAttacked;
    private bool death;
    public float attackTime;
    private float invulnTime;
    public float randomAttack;
    //False represent left drone, true represent right drone
    public bool droneDirection;
    [SerializeField] private GameObject character;
    [SerializeField] private int droneHP;
    [SerializeField] private int droneSpeed;
    [SerializeField] private float interval;
    //Default material
    private Material defaultMaterial;
    //White material
    [SerializeField] private Material whiteMaterial;

    private void OnEnable()
    {
        //Reset drone value and states
        droneHP = 300;
        attackTime = 0f;
        attacking = false;
        isAttacked = false;
        death = false;
        randomAttack = Random.Range(0f, 1f);

        //Spawn drone either from top or bottom
        if (Random.Range(0f, 1f) > 0.5f)
        {
            //Spawn from top
            transform.localPosition = new Vector3(transform.localPosition.x, 12f, 0);
        }
        else
        {
            //Spawn from bottom
            transform.localPosition = new Vector3(transform.localPosition.x, -12f, 0);
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultMaterial = sr.material;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!attacking)
        {
            //Get character view port position from world position
            Vector3 viewPos = Camera.main.WorldToViewportPoint(character.transform.position);

            //Scale character's y view port position to drone's local y position as the target position
            float targetY = -10f + (20f * viewPos.y);

            //Restrict drone local y position to be between -8f and 8f
            float localY = Mathf.Clamp(targetY, -8f, 8f);

            //Move drone's local y position towards local target y position
            float towardsY = Mathf.MoveTowards(transform.localPosition.y, localY, droneSpeed * Time.deltaTime);

            //Set new local position for drone
            transform.localPosition = new Vector3(transform.localPosition.x, towardsY, 10f);

            if(attackTime > interval)
            {
                attacking = true;
                if(randomAttack < 0.9f)
                {
                    Laser();
                }
                else
                {
                    Beam();
                }
                //Refresh next random attack
                randomAttack = Random.Range(0f, 1f);
            }
            else
            {
                attackTime += Time.deltaTime;
            }
        }

        //Drone cannot be attacked
        if (invulnTime > 0)
        {
            invulnTime -= Time.deltaTime;
        }
        //Drone can be attacked
        else
        {
            //Change back to default material
            sr.material = defaultMaterial;
            isAttacked = false;
        }

        //Drone died
        if (droneHP <= 0 && !death)
        {
            death = true;
            animator.SetTrigger("death");
        }

        ////Get character view port position from world position
        //Vector3 viewPos = Camera.main.WorldToViewportPoint(character.transform.position);

        ////Scale character's y view port position to drone's local y position as the target position
        //float targetY = -10f + (20f * viewPos.y);

        ////Restrict drone local y position to be between -8f and 8f
        //float localY = Mathf.Clamp(targetY, -8f, 8f);

        ////Move drone's local y position towards local target y position
        //float towardsY = Mathf.MoveTowards(transform.localPosition.y, localY, droneSpeed * Time.deltaTime);

        ////Set new local position for drone
        //transform.localPosition = new Vector3(transform.localPosition.x, towardsY, 10f);
    }

    private void Laser()
    {
        animator.SetTrigger("laser");
    }

    private void Beam()
    {
        animator.SetTrigger("beam_start");
    }

    public void DroneAttacked(int amount, float seconds)
    {
        if(!isAttacked)
        {
            // Drone flashes white
            sr.material = whiteMaterial;
            isAttacked = true;
            droneHP -= amount;
            invulnTime = seconds;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            DroneAttacked(20, 0.05f);
        }
    }
}
