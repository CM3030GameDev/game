using UnityEngine;

public class EMPFieldWeapon : SecondaryWeapon
{
    // Scale root only - keep animated sprites as CHILDREN of this, never on it. An Animator
    // writing localScale on the same object would overwrite whatever ScaleVisual() sets.
    [SerializeField] private Transform fieldVisual;
    [SerializeField] private float combinedStunChance = 0.2f;      // combined form only
    [SerializeField] private float combinedStunDuration = 1.5f;

    private static int EnemyMask;

    private void Awake() => EnemyMask = LayerMask.GetMask("Enemy");

    protected override void OnInit() => ScaleVisual();
    protected override void OnLevelChanged() => ScaleVisual();

    private void ScaleVisual()
    {
        if (fieldVisual != null)
            fieldVisual.localScale = Vector3.one * (Stats.valueA * 2f);
    }

    // Pure crowd control - the field deals no damage, it only slows what's inside it.
    // valueA = field radius, valueB = speed multiplier while slowed.
    protected override void Fire()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Stats.valueA, EnemyMask);
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob == null) continue;

            // Combined form: chance to stop a mob dead rather than merely slowing it.
            // A stun is just a zero-speed debuff, so it reuses the same system.
            if (IsCombined && Random.value < combinedStunChance)
            {
                mob.ApplyDebuff(0f, combinedStunDuration);
                continue;
            }

            mob.ApplyDebuff(Stats.valueB, Stats.fireInterval * 1.5f);
        }
    }
}
