using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    private int damage = 50;
    private float blastRadius = 1.5f;

    private static int EnemyLayer;
    private static int EnemyMask;

    public void Configure(int dmg, float radius)
    {
        damage = dmg;
        blastRadius = radius;
    }

    private void Awake()
    {
        EnemyLayer = LayerMask.NameToLayer("Enemy");
        EnemyMask = LayerMask.GetMask("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != EnemyLayer) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius, EnemyMask);
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob != null) mob.MobAttacked(damage, 0.1f);
        }

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}