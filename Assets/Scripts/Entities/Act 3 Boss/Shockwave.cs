using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Animator animator;
    private Character character;
    private bool knockBack;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject soldier;
    [SerializeField] private float knockbackForce;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = soldier.GetComponent<Character>();
    }

    private void OnEnable()
    {
        knockBack = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Disable shockwave effect
            gameObject.SetActive(false);
            //Boss attack ending animation
            bossAnimator.SetBool("attack", false);
        }
    }

    private void LateUpdate()
    {
        if (knockBack)
        {
            //Direction vector from boss to character
            Vector2 direction = soldier.transform.position - transform.position;
            Vector2 normalizedDirection = direction.normalized;

            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            //Knockback effect from shockwave
            rb.AddForce(normalizedDirection * knockbackForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            character.CharacterAttacked(50, 0.5f);
            knockBack = true;
        }
    }
}
