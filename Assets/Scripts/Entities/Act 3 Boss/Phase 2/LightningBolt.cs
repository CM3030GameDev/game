using UnityEngine;

public class LightningBolt : MonoBehaviour
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
            character.CharacterAttacked(damage, 0.05f);

            //Stunned status effect
            PhaseTwoManager.Instance.Stunned();

            //Disable lightning bolt projectile
            gameObject.SetActive(false);
            //Return lightning bolt projectile back to object pool
            PhaseTwoManager.Instance.projectiles.Enqueue(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            //Disable lightning bolt projectile
            gameObject.SetActive(false);
            //Return lightning bolt projectile back to object pool
            PhaseTwoManager.Instance.projectiles.Enqueue(gameObject);
        }
    }
}
