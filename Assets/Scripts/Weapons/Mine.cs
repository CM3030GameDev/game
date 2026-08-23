using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private int damage = 50;
    private float blastRadius = 1.5f;

    public void Configure(int dmg, float radius)
    {
        damage = dmg;
        blastRadius = radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (var h in hits)
        {
            if (!h.CompareTag("Enemy")) continue;
            Mob mob = h.GetComponent<Mob>();
            if (mob != null) mob.MobAttacked(damage, 0.1f);
        }

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}