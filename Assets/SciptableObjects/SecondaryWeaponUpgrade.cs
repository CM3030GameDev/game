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

    // Describe the level this pick would actually grant, not the weapon in general.
    public override string GetDescription(UpgradeContext ctx)
    {
        SecondaryWeapon owned = ctx.slots != null ? ctx.slots.Find(weapon) : null;
        int nextLevel = owned == null ? 1 : owned.Level + 1;

        if (nextLevel >= 1 && nextLevel <= weapon.levels.Length)
        {
            string levelText = weapon.levels[nextLevel - 1].levelDescription;
            if (!string.IsNullOrWhiteSpace(levelText)) return levelText;
        }

        return description;   // fall back to the generic line if that level has no text
    }

    // Names the stat this weapon evolves with, and flags when that stat is already maxed.
    public override string GetComboText(UpgradeContext ctx)
    {
        CharacterStatsUpgrade partner = Partner();
        if (partner == null) return null;

        return ComboLine(partner.upgradeName, IsComboReady(ctx));
    }

    // The player has taken the paired stat at least once.
    public override bool OwnsComboPartner(UpgradeContext ctx)
    {
        CharacterStatsUpgrade partner = Partner();
        return partner != null && ctx.statLevels != null && ctx.statLevels.GetLevel(partner) > 0;
    }

    // The paired stat is maxed, so this weapon is one finished track from evolving.
    public override bool IsComboReady(UpgradeContext ctx)
    {
        CharacterStatsUpgrade partner = Partner();
        return partner != null && ctx.statLevels != null && ctx.statLevels.IsMaxLevel(partner);
    }

    // The weapon data owns the icon, so card, HUD slot and combo badges all show the same art.
    public override Sprite GetIcon(UpgradeContext ctx)
        => weapon != null && weapon.icon != null ? weapon.icon : icon;

    public override Sprite GetComboIcon(UpgradeContext ctx)
    {
        CharacterStatsUpgrade partner = Partner();
        return partner != null ? partner.icon : null;
    }

    private CharacterStatsUpgrade Partner()
    {
        if (weapon == null || weapon.isCombinedForm) return null;
        if (weapon.combinedResult == null) return null;
        return weapon.combinesWithStat;
    }
}