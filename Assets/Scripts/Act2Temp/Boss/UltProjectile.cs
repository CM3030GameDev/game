using UnityEngine;

public class UltProjectile : MonoBehaviour
{
    private int damage = 15;
    private float lifetime = 5f;
    private float speed;

    private Vector3 moveDir;
    private bool isLaunched = false;

    public void Launch(Vector3 direction, float launchSpeed, float lifeTimer, int damageDone)
    {
        damage = damageDone;
        lifetime = lifeTimer;
        moveDir = direction;
        speed = launchSpeed;
        isLaunched = true;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!isLaunched)
            return;
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character") && collision.isTrigger)
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
                character.CharacterAttacked(damage);
            Destroy(gameObject);
        }
    }
}
