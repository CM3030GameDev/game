using UnityEngine;

public class EMPFieldWeapon : SecondaryWeapon
{
    [SerializeField] private SpriteRenderer fieldVisual;

    protected override void OnInit() => ScaleVisual();
    protected override void OnLevelChanged() => ScaleVisual();

    private void ScaleVisual()
    {
        if (fieldVisual != null)
            fieldVisual.transform.localScale = Vector3.one * (Stats.valueA * 2f);
    }

    protected override void Fire()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Stats.valueA);
        foreach (var h in hits)
        {
            if (!h.CompareTag("Enemy")) continue;
            Mob mob = h.GetComponent<Mob>();
            if (mob != null)
            {
                mob.MobAttacked(Stats.damage, 0.05f);
                mob.ApplyDebuff(Stats.valueB, Stats.fireInterval * 1.5f);
            }
        }
    }
}