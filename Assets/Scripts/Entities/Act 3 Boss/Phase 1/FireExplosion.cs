using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int damage;

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
        //Fire explosion animation finished
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Disable explosion
            gameObject.SetActive(false);
            //Return explosion object back to object pool
            PhaseOneManager.Instance.explosions.Enqueue(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.5f);
        }
    }
}
