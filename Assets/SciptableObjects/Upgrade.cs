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

    // Maps a stat to the weapon it evolves. UpgradeManager builds it so the pairing cannot drift.
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

    // The "Combines with X" line, kept separate so the card can style it as its own row.
    public virtual string GetComboText(UpgradeContext ctx) => null;

    // True when the player already holds the other half at any level. Drives the card highlight.
    public virtual bool OwnsComboPartner(UpgradeContext ctx) => false;

    // True when the other half is maxed. Drives the wording, which is a progress readout.
    public virtual bool IsComboReady(UpgradeContext ctx) => false;

    // Shared wording so both card types phrase the combination hint identically.
    protected static string ComboLine(string partnerName, bool partnerReady)
        => partnerReady
            ? $"Ready to combine with {partnerName}"
            : $"Combines with {partnerName}";

    public abstract void Apply(UpgradeContext ctx);
}