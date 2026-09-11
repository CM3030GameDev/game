using UnityEngine;

public class GroundImpact : MonoBehaviour
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
        //Ground impact animation finished
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Disable ground impact attack
            gameObject.SetActive(false);
            //Return impact gameobject back to object pool
            PhaseTwoManager.Instance.impacts.Enqueue(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(50);
            character.GrantInvulnerability(0.5f);
        }
    }
}
