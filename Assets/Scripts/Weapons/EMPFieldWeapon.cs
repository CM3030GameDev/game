using UnityEngine;

public class EMPFieldWeapon : SecondaryWeapon
{
    [SerializeField] private SpriteRenderer fieldVisual;

    private static int EnemyMask;

    private void Awake() => EnemyMask = LayerMask.GetMask("Enemy");

    protected override void OnInit() => ScaleVisual();
    protected override void OnLevelChanged() => ScaleVisual();

    private void ScaleVisual()
    {
        if (fieldVisual != null)
            fieldVisual.transform.localScale = Vector3.one * (Stats.valueA * 2f);
    }

    protected override void Fire()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Stats.valueA, EnemyMask);
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob != null)
            {
                mob.MobAttacked(TotalDamage, 0.05f);
                mob.ApplyDebuff(Stats.valueB, Stats.fireInterval * 1.5f);
            }
        }
    }
}