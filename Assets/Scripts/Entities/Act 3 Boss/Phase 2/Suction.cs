using UnityEngine;

public class Suction : MonoBehaviour
{
    private Animator animator;
    //Force multiplier that scales with distance (Closer stronger, further weaker)
    private float multiplier;
    [SerializeField] private LayerMask layers;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject shockwave;
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
            //Disable suction effect
            gameObject.SetActive(false);
            //Disable hitbox indicator
            range.SetActive(false);
            //Enable shockwave effect
            shockwave.SetActive(true);
        }

        //Direction vector from boss to character
        Vector2 direction = PhaseTwoManager.Instance.characterTransform.position - transform.position;
        Vector2 normalizedDirection = direction.normalized;

        //Raycast to check for either player or obstacle collision
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100f, layers.value);

        //If raycast hits a collider
        if(hit)
        {
            //Pull player if collider detected is character
            if(hit.collider.CompareTag("Character"))
            {
                //Distance between boss and character
                float distance = Vector2.Distance(transform.position, PhaseTwoManager.Instance.characterTransform.position);
                //Multiplier effect of force (Further weaker, closer stronger)
                multiplier = (100f - distance) / 100f;

                Rigidbody2D rb = PhaseTwoManager.Instance.character.GetComponent<Rigidbody2D>();
                //Force applied on player in the direction vector towards boss
                rb.AddForce(normalizedDirection * maxForce * multiplier * -1, ForceMode2D.Force);
            }
        }
    }
}
