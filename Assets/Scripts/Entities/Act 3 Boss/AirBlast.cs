using UnityEngine;

public class AirBlast : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Destroy air blast object automatically after 2 seconds
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
            character.CharacterAttacked(10);
            character.GrantInvulnerability(0.05f);
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
