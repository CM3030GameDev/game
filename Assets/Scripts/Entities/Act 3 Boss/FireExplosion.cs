using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    private Animator animator;

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
            //Disable fire attack
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(50, 0.5f);
        }
    }
}
