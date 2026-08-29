using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Upgrades/Secondary Weapon")]
public class SecondaryWeaponUpgrade : Upgrade
{
    public SecondaryWeaponData weapon;

    public override bool IsAvailable(UpgradeContext ctx)
    {
        return ctx.slots != null && ctx.slots.CanOffer(weapon);
    }

    public override void Apply(UpgradeContext ctx)
    {
        ctx.slots.AcquireOrLevel(weapon);
    }
}