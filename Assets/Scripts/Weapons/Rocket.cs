using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float lifetime = 4f;

    private int damage = 60;
    private float blastRadius = 2f;

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

    private void Start() => Destroy(gameObject, lifetime);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != EnemyLayer) return;
        Detonate();
    }

    private void Detonate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius, EnemyMask);
        foreach (var h in hits)
        {
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