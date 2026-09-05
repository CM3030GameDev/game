using UnityEngine;

public class WindBlast : MonoBehaviour
{
    [SerializeField] private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Destroy wind blast object automatically after 2 seconds
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.05f);

            //Character confused status effect function add in character script

            Destroy(gameObject);
        }
        else if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
