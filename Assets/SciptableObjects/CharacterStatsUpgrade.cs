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

    // Names the weapon this stat evolves, and flags when that weapon is already maxed.
    public override string GetComboText(UpgradeContext ctx)
    {
        SecondaryWeaponData partner = Partner(ctx, out SecondaryWeapon owned);
        if (partner == null) return null;

        bool weaponMaxed = owned != null && owned.IsMaxLevel;
        return ComboLine(partner.weaponName, weaponMaxed);
    }

    public override Sprite GetComboIcon(UpgradeContext ctx)
    {
        SecondaryWeaponData partner = Partner(ctx, out _);
        return partner != null ? partner.icon : null;
    }

    private SecondaryWeaponData Partner(UpgradeContext ctx, out SecondaryWeapon owned)
    {
        owned = null;
        if (ctx.statPartners == null) return null;
        if (!ctx.statPartners.TryGetValue(this, out SecondaryWeaponData partner)) return null;

        owned = ctx.slots != null ? ctx.slots.Find(partner) : null;
        if (owned != null && owned.IsCombined) return null;   // already evolved, nothing left to hint

        return partner;
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