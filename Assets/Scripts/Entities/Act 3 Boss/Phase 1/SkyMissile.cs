using UnityEngine;
using System.Collections;

public class SkyMissile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject indicator;
    //Target state
    private bool targeted;
    //Impact state
    private bool impact;
    //Target position randomly on current screen position
    private Vector3 targetPos;
    private float missileSpeed;
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //Random range to vary missile speed
        missileSpeed = Random.Range(speedMin, speedMax);
        targeted = false;
        impact = false;
        //Reset target position to a position that is out of bounds of map
        targetPos = Vector3.negativeInfinity;
        StartCoroutine(DescentTime(2f));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Explosion at targeted area
        if (!impact && transform.position.y <= targetPos.y)
        {
            impact = true;
            //Disable hitbox indicator
            indicator.SetActive(false);
            //Return indicator object back to object pool
            PhaseOneManager.Instance.indicators.Enqueue(indicator);
            //Set rotation of missile back to zero
            transform.rotation = Quaternion.identity;
            //Start missile explosion animation
            animator.SetTrigger("explode");
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Missile_Barrage_Explosion") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Disable missile object
            gameObject.SetActive(false);
            //Return missile object back to object pool
            PhaseOneManager.Instance.missiles.Enqueue(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(targeted)
        {
            targeted = false;
            //Random position on the current screen
            targetPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Camera.main.nearClipPlane));
            //Get indicator object from object pool
            indicator = PhaseOneManager.Instance.indicators.Dequeue();
            //Set indicator to appear at target position
            indicator.transform.position = targetPos;
            //Scale indicator to hitbox range of missile explosion
            indicator.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            //Start showing indicator at target position
            indicator.SetActive(true);
            //Position missile such that it angles 45 degree downwards towards targeted position
            rb.MovePosition((Vector2)targetPos + new Vector2(-30f, 30f));
            //Angle missile at 45 degree downwards
            transform.rotation = Quaternion.Euler(0f, 0f, -45f);
        }

        //Stop moving missile at target position
        if(impact)
        {
            rb.linearVelocity = Vector2.zero;
        }
        //Continue moving until it reaches target position
        else
        {
            rb.linearVelocity = transform.right * missileSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.5f);
        }
    }

    //Time until it starts its descent to targeted position
    IEnumerator DescentTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        targeted = true;
    }
}
