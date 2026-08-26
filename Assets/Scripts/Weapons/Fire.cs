using UnityEngine;

public class Fire : MonoBehaviour
{
    private BoxCollider2D box;
    private SpriteRenderer sr;

    private int damage = 2;
    private float damageCD = 0.2f;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void SetDamage(int dmg, float dmgCD)
    {
        damage = dmg;
        damageCD = dmgCD;
    }

    // Update is called once per frame
    void Update()
    {
        //Dynamically change hitbox according to each sprite in animation
        box.size = sr.sprite.bounds.size;
        //Offset by half the sprite size since each sprite pivot is left
        box.offset = new Vector2(box.size.x / 2, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IDamageable target = collision.GetComponent<IDamageable>();
            target?.TakeDamage(damage, damageCD);
        }
    }
}
