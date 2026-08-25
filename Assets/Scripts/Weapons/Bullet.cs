using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float hitFlash = 0.05f;
    [SerializeField] private int pierceCount = 1;
    [SerializeField] private float lifetime = 2f;

    private int hitsRemaining;
    private float knockbackForce;

    private void Awake() { hitsRemaining = pierceCount; }

    private void Start() { Destroy(gameObject, lifetime); }

    public void SetDamage(int d) { damage = d; }
    public void SetKnockback(float force) => knockbackForce = force;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Mob mob = other.GetComponent<Mob>();
            if (mob != null)
            {
                mob.MobAttacked(damage, hitFlash);

                if (knockbackForce > 0f)
                {
                    Vector2 dir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
                    mob.Knockback(dir, knockbackForce);
                }
            }
        }
        //Final boss collision
        else if (other.CompareTag("FinalBoss"))
        {
            FinalBoss finalBoss = other.GetComponent<FinalBoss>();
            if (finalBoss != null) finalBoss.BossAttacked(damage, hitFlash);
        }
        //Character collision (Final boss barrier reflected bullet)
        else if (other.CompareTag("Character"))
        {
            Character character = other.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(hitFlash);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else
        {
            return;   // irrelevant collider - don't burn a pierce charge
        }

        hitsRemaining--;
        if (hitsRemaining <= 0) Destroy(gameObject);
    }
}