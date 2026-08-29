using UnityEngine;

public class Suction : MonoBehaviour
{
    private Animator animator;
    //Force multiplier that scales with distance (Closer stronger, further weaker)
    private float multiplier;
    [SerializeField] private LayerMask layers;
    [SerializeField] private Character character;
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private GameObject wind;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject explosionTop;
    [SerializeField] private GameObject explosionBottom;
    [SerializeField] private GameObject explosionLeft;
    [SerializeField] private GameObject explosionRight;
    [SerializeField] private float maxForce;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //After 10 cycles
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 10f)
        {
            //Disable suction, wind and range effect
            gameObject.SetActive(false);
            wind.SetActive(false);
            range.SetActive(false);
            explosionTop.SetActive(true);
            explosionBottom.SetActive(true);
            explosionLeft.SetActive(true);
            explosionRight.SetActive(true);
            //Reset attack probability and timer
            finalBoss.randomNum = Random.Range(0f, 1f);
            finalBoss.attackTime = 0f;
        }

        //Direction vector from boss to character
        Vector2 direction = character.transform.position - transform.position;
        Vector2 normalizedDirection = direction.normalized;

        //Raycast to check if player can be pulled
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100f, layers.value);

        //If raycast hits a collider
        if(hit)
        {
            //Pull player if collider detected is character
            if(hit.collider.CompareTag("Character"))
            {
                //Distance between boss and character
                float distance = Vector2.Distance(transform.position, character.transform.position);
                //Multiplier effect of force (Further weaker, closer stronger)
                multiplier = (100f - distance) / 100f;

                Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
                rb.AddForce(normalizedDirection * maxForce * multiplier * -1, ForceMode2D.Force);
            }
        }
    }
}
