using UnityEngine;

public enum CharacterStatsType { MaxHealth, MoveSpeed, AttackSpeed, PickupRadius }

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrades/Stat")]
public class CharacterStatsUpgrade : Upgrade
{
    public CharacterStatsType stat;
    public float[] amountPerLevel = new float[3]; // 3 max upgrades for each stat (Lvl 1 -> 2 -> Max (3))
    public int maxLevel = 3;

    public override bool IsAvailable(UpgradeContext ctx)
    {
        return ctx.statLevels.GetLevel(this) < maxLevel;
    }

    public override void Apply(UpgradeContext ctx)
    {
        int newLevel = ctx.statLevels.Increment(this);
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
                break;
        }
    }
}