using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Mob mob = collision.gameObject.GetComponent<Mob>();
            mob.MobAttacked(20, 0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Mob mob = collision.GetComponent<Mob>();
            mob.MobAttacked(20, 0.05f);
        }
    }
}
