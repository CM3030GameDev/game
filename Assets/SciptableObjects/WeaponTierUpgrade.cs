using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrades/WeaponTier")]
public class WeaponTierUpgrade : Upgrade
{
    public WeaponTier tier;

    public override void Apply(UpgradeContext ctx)
    {
        ctx.weapon.SetTier(tier);
    }
}
