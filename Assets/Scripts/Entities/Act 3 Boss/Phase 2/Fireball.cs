using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.05f);

            //Burnt status effect
            PhaseTwoManager.Instance.Burnt();

            //Disable fireball projectile
            gameObject.SetActive(false);
            //Return fireball projectile back to object pool
            PhaseTwoManager.Instance.projectiles.Enqueue(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            //Disable fireball projectile
            gameObject.SetActive(false);
            //Return fireball projectile back to object pool
            PhaseTwoManager.Instance.projectiles.Enqueue(gameObject);
        }
    }
}
