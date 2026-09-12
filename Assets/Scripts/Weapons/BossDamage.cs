using UnityEngine;

// Lets area weapons (rocket, mine, drone) damage the same bosses Bullet already hits.
// The bosses share no base class, so each type is handled here once instead of in every weapon.
public static class BossDamage
{
    private static int mask;

    // Act 1 and Act 2 bosses are on Enemy; both final boss phases are on Default.
    public static int Mask => mask != 0 ? mask : (mask = LayerMask.GetMask("Enemy", "Default"));

    // InParent, so a boss's child colliders (like an attack range) still resolve to the boss.
    public static MonoBehaviour Find(Collider2D c)
    {
        MonoBehaviour boss = c.GetComponentInParent<Act1Boss>();
        if (boss == null) boss = c.GetComponentInParent<Act2Miniboss>();
        if (boss == null) boss = c.GetComponentInParent<FinalBossOne>();
        if (boss == null) boss = c.GetComponentInParent<FinalBossTwo>();
        return boss;
    }

    public static void Damage(MonoBehaviour boss, int amount, float flash)
    {
        switch (boss)
        {
            case Act1Boss a1: a1.BossAttacked(amount, flash); break;
            case Act2Miniboss a2: a2.MobAttacked(amount, flash); break;
            case FinalBossOne f1: f1.BossAttacked(amount, flash); break;
            case FinalBossTwo f2: f2.BossAttacked(amount, flash); break;
        }
    }
}
