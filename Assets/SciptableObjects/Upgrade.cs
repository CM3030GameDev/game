using System.Collections.Generic;
using UnityEngine;

public class UpgradeContext
{
    public CharacterStats stats;
    public MainWeapon weapon;
    public SoldierSkill skill;
    public WeaponSlots slots;
    public StatLevels statLevels;
    public Color cardColor = Color.black;

    // stat -> the weapon it evolves. Only SecondaryWeaponData knows its partner, so
    // UpgradeManager builds the reverse map once and stat cards read it from here.
    public Dictionary<CharacterStatsUpgrade, SecondaryWeaponData> statPartners
        = new Dictionary<CharacterStatsUpgrade, SecondaryWeaponData>();
}

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName = "Upgrade";
    [TextArea] public string description = "";
    public Sprite icon;

    // Check if you can still upgrade stuff
    public virtual bool IsAvailable(UpgradeContext ctx) => true;

    // What the upgrade card shows. Override to describe the specific level being offered.
    public virtual string GetDescription(UpgradeContext ctx) => description;

    // Icon shown on the card. Override where the real source of truth lives elsewhere.
    public virtual Sprite GetIcon(UpgradeContext ctx) => icon;

    // Icon of the partner this combines with, or null when there's no pairing to show.
    public virtual Sprite GetComboIcon(UpgradeContext ctx) => null;

    // "Combines with X" line, or null. Kept separate from GetDescription so the card can
    // put it in its own styled row; UpgradeCardUI appends it to the description if there isn't one.
    public virtual string GetComboText(UpgradeContext ctx) => null;

    // Shared wording so both card types phrase the combination hint identically.
    protected static string ComboLine(string partnerName, bool partnerReady)
        => partnerReady
            ? $"Ready to combine with {partnerName}"
            : $"Combines with {partnerName}";

    public abstract void Apply(UpgradeContext ctx);
}