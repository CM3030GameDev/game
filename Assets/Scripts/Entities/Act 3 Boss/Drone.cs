using UnityEngine;

public class Drone : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sr;
    private Animator animator;
    private float droneSpeed;
    private float offsetX;
    public bool attacking;
    public bool beaming;
    private bool isAttacked;
    private bool death;
    public float attackTime;
    private float invulnTime;
    public float randomAttack;
    //False represent left drone, true represent right drone
    public bool droneDirection;
    [SerializeField] private GameObject character;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private int droneHP;
    [SerializeField] private float interval;
    //Default material
    private Material defaultMaterial;
    //White material
    [SerializeField] private Material whiteMaterial;

    private void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        defaultMaterial = sr.material;
    }

    private void OnEnable()
    {
        //Reset drone value and states
        droneHP = 300;
        droneSpeed = Random.Range(characterStats.moveSpeed - 2, characterStats.moveSpeed);
        attackTime = 0f;
        attacking = false;
        beaming = false;
        isAttacked = false;
        death = false;
        randomAttack = Random.Range(0f, 1f);

        //Right drone
        if(droneDirection)
        {
            offsetX = 13f;
        }
        //Left drone
        else
        {
            offsetX = -13f;
        }

        //Spawn drone either from top or bottom
        if (Random.Range(0f, 1f) > 0.5f)
        {
            //Spawn from top
            transform.localPosition = new Vector3(cam.transform.position.x + offsetX, cam.transform.position.y + 12f, 0f);
        }
        else
        {
            //Spawn from bottom
            transform.localPosition = new Vector3(cam.transform.position.x + offsetX, cam.transform.position.y - 12f, 0f);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////Get character view port position from world position
        //Vector3 viewPos = Camera.main.WorldToViewportPoint(character.transform.position);

        ////Converting character's y view port position to local y position
        //float targetY = -10f + (20f * viewPos.y);

        ////Restrict drone local y position to be between -8f and 8f
        //float localY = Mathf.Clamp(targetY, -8f, 8f);

        ////Move drone's local y position towards local target y position
        //float towardsY = Mathf.MoveTowards(transform.localPosition.y, localY, droneSpeed * Time.deltaTime);

        ////Set new local position for drone
        //transform.localPosition = new Vector3(transform.localPosition.x, towardsY, 10f);

        //Drone died
        if (droneHP <= 0 && !death)
        {
            death = true;
            animator.SetTrigger("death");
        }
    }

    private void LateUpdate()
    {
        if (!attacking)
        {
            //Move drone's y position towards player
            float towardsY = Mathf.MoveTowards(transform.position.y, character.transform.position.y, droneSpeed * Time.deltaTime);
            //Drone's x position is restricted to left/right corner of the screen while y position move according to player position
            transform.position = new Vector3(cam.transform.position.x + offsetX, towardsY, 0f);

            if (attackTime > interval)
            {
                attacking = true;
                if (randomAttack < 0.9f)
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
        else
        {
            //Restrict drone from moving its y position when beaming
            if(beaming)
            {
                //Drone's x position is restricted to left/right corner of the screen while y position is restricted from moving when attacking
                transform.position = new Vector3(cam.transform.position.x + offsetX, transform.position.y, 0f);
            }
            //Allow drone to move its y position when shooting laser
            else
            {
                //Move drone's y position towards player
                float towardsY = Mathf.MoveTowards(transform.position.y, character.transform.position.y, droneSpeed * Time.deltaTime);
                //Drone's x position is restricted to left/right corner of the screen while y position move according to player position
                transform.position = new Vector3(cam.transform.position.x + offsetX, towardsY, 0f);
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
    }

    private void Laser()
    {
        animator.SetTrigger("laser");
    }

    private void Beam()
    {
        animator.SetTrigger("beam_start");
        beaming = true;
    }

    public void DroneAttacked(int amount, float seconds)
    {
        if(!isAttacked)
        {
            //Drone flashes white
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
