using UnityEngine;

public class SoldierSkillExplosion : MonoBehaviour
{
    [SerializeField] private int damage = 50;
    [SerializeField] private float radius = 1.5f;
    [SerializeField] private float hitFlash = 0.1f;
    [SerializeField] private float lifetime = 0.6f;

    private void Start()
    {
        // Damage everything in radius of the explosion
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var h in hits)
        {
            if (!h.CompareTag("Enemy")) continue;
            Mob mob = h.GetComponent<Mob>();
            if (mob != null) mob.MobAttacked(damage, hitFlash);
        }
        Destroy(gameObject, lifetime);
    }
}