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

    public void SetDamage(int d) { damage = d; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Mob mob = other.GetComponent<Mob>();
            FinalBoss finalBoss = other.GetComponent<FinalBoss>();
            if (mob != null)
            {
                mob.MobAttacked(damage, hitFlash);
                hitsRemaining--;
            }
            else if (finalBoss != null)
            {
                finalBoss.BossAttacked(damage, hitFlash);
                hitsRemaining--;
            }

            if (hitsRemaining <= 0)
            {
                Destroy(gameObject);
            }
        }
        //Reflected bullet (From final boss's barrier)
        else if(other.CompareTag("Character"))
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
    }
}