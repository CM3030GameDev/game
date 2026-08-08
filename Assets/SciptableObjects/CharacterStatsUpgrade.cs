using UnityEngine;

public enum CharacterStatsType { MaxHealth, MoveSpeed, AttackSpeed, PickupRadius }

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrades/Stat")]
public class CharacterStatsUpgrade : Upgrade
{
    public CharacterStatsType stat;
    public float amount = 1f;

    public override void Apply(UpgradeContext ctx)
    {
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
        }
    }

    public override bool IsAvailable(UpgradeContext ctx)
    {
        if (stat == CharacterStatsType.AttackSpeed && ctx.stats.attackSpeed >= 0.8f) return false;
        return true;
    }
}
