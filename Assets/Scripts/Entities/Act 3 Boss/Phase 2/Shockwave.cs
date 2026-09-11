using UnityEngine;

public class Shockwave : MonoBehaviour
{
    private Animator animator;
    private bool knockBack;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private float knockbackForce;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            Vector2 direction = PhaseTwoManager.Instance.characterTransform.position - transform.position;
            Vector2 normalizedDirection = direction.normalized;

            Rigidbody2D rb = PhaseTwoManager.Instance.character.GetComponent<Rigidbody2D>();
            //Knockback effect from shockwave
            rb.AddForce(normalizedDirection * knockbackForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            PhaseTwoManager.Instance.character.CharacterAttacked(50);
            PhaseTwoManager.Instance.character.GrantInvulnerability(0.5f);
            knockBack = true;
        }
    }
}
