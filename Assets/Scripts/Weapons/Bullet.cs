using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float hitFlash = 0.05f;
    [SerializeField] private int pierceCount = 1;
    [SerializeField] private float lifetime = 2f;

    private int hitsRemaining;

    private void Awake() { hitsRemaining = pierceCount; }

    private void Start() { Destroy(gameObject, lifetime); }
    private float knockbackForce;

    public void SetDamage(int d) { damage = d; }
    public void SetKnockback(float force) => knockbackForce = force;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Mob collision
        if(other.CompareTag("Mobs"))
        {
            Mob mob = other.GetComponent<Mob>();
            mob.MobAttacked(damage, hitFlash);
            if (knockbackForce > 0f)
            {
                Vector2 dir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                mob.Knockback(dir, knockbackForce);
            }
            hitsRemaining--;
        }
        //Final boss collision
        else if(other.CompareTag("FinalBoss"))
        {
            FinalBoss finalBoss = other.GetComponent<FinalBoss>();
            finalBoss.BossAttacked(damage, hitFlash);
            hitsRemaining--;
        }
        //Character collision (Final boss barrier reflected bullet)
        else if (other.CompareTag("Character"))
        {
            Character character = other.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(hitFlash);
            hitsRemaining--;
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (hitsRemaining <= 0) Destroy(gameObject);
    }
}