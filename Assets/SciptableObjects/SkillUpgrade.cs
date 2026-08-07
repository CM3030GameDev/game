using UnityEngine;

public enum SkillStat { Cooldown, Damage, StrikeCount }

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrades/Skill")]
public class SkillUpgrade : Upgrade
{
    public SkillStat stat;
    public float amount = 1f;

    public override void Apply(UpgradeContext ctx)
    {
        ctx.skill.ApplyUpgrade(stat, amount);
    }
}