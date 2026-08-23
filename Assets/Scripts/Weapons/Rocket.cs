using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float lifetime = 4f;

    private int damage = 60;
    private float blastRadius = 2f;

    public void Configure(int dmg, float radius)
    {
        damage = dmg;
        blastRadius = radius;
    }

    private void Start() => Destroy(gameObject, lifetime);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        Detonate();
    }

    private void Detonate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (var h in hits)
        {
            if (!h.CompareTag("Enemy")) continue;
            Mob mob = h.GetComponent<Mob>();
            if (mob != null) mob.MobAttacked(damage, 0.1f);
        }

        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            fx.transform.localScale = Vector3.one * (blastRadius / 1.5f);
        }

        Destroy(gameObject);
    }
}