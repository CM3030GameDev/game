using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float hitFlash = 0.05f;
    [SerializeField] private int pierceCount = 1;
    [SerializeField] private float lifetime = 2f;

    private int hitsRemaining;
    private float knockbackForce;
    private bool isReflected;
    private float stunChance;
    private float stunDuration;

    private void Awake() { hitsRemaining = pierceCount; }

    private void Start() { Destroy(gameObject, lifetime); }

    public void SetDamage(int d) { damage = d; }
    public void SetKnockback(float force) => knockbackForce = force;
    public void SetStun(float chance, float duration) { stunChance = chance; stunDuration = duration; }
    public void MarkReflected() { isReflected = true; }

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

                // Stun is a zero-speed debuff - reuses the existing slow system
                if (stunChance > 0f && Random.value < stunChance)
                    mob.ApplyDebuff(0f, stunDuration);
            }
        }
        // Final Boss damage check
        else if (other.CompareTag("FinalBoss"))
        {
            FinalBoss finalBoss = other.GetComponent<FinalBoss>();
            finalBoss.BossAttacked(damage, hitFlash);
            hitsRemaining--;
        }
        // Act 1 miniboss damage check
        else if (other.CompareTag("MiniBoss"))
        {
            Act1Boss miniboss = other.GetComponent<Act1Boss>();
            miniboss.BossAttacked(damage, hitFlash);
            hitsRemaining--;
        }
        // Act 2 miniboss damage check
        else if (other.CompareTag("MiniBoss2"))
        {
            Act2Miniboss miniboss = other.GetComponent<Act2Miniboss>();
            miniboss.MobAttacked(damage, hitFlash);
            hitsRemaining--;
        }
        // Character damage check - only if a barrier reflected this bullet back at the player;
        // otherwise a weapon spawning bullets near the player would hit them immediately.
        else if (isReflected && other.CompareTag("Character"))
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
        else
        {
            return;
        }

        if (hitsRemaining <= 0) Destroy(gameObject);
    }
}