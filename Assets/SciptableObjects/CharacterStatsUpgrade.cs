using UnityEngine;

public enum CharacterStatsType { MaxHealth, MoveSpeed, AttackSpeed, PickupRadius, Damage, HealthRegen }

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrades/Stat")]
public class CharacterStatsUpgrade : Upgrade
{
    public CharacterStatsType stat;
    public float[] amountPerLevel = new float[3];

    public int MaxLevel => amountPerLevel.Length;

    public override bool IsAvailable(UpgradeContext ctx)
    {
        return ctx.statLevels != null && ctx.statLevels.CanOffer(this);
    }

    public override void Apply(UpgradeContext ctx)
    {
        int newLevel = ctx.statLevels.Increment(this);
        ctx.statLevels.RefreshSlot(this);
        float amount = amountPerLevel[newLevel - 1];

        switch (stat)
        {
            case CharacterStatsType.MaxHealth:
                ctx.stats.maxHealth += (int)amount;
                ctx.stats.health += (int)amount;
                break;
            case CharacterStatsType.MoveSpeed:
                ctx.stats.moveSpeed += amount;
                break;
            case CharacterStatsType.AttackSpeed:
                ctx.stats.attackSpeed = Mathf.Min(0.8f, ctx.stats.attackSpeed + amount);
                break;
            case CharacterStatsType.PickupRadius:
                ctx.stats.pickupRadius += amount;
                break;
            case CharacterStatsType.Damage:
                ctx.stats.damage += amount;
                break;
            case CharacterStatsType.HealthRegen:
                ctx.stats.healthRegen += amount;
                break;
        }
    }
}