using UnityEngine;

// Show what can be upgraded
public class UpgradeContext
{
    public CharacterStats stats;
    public MainWeapon weapon;
    public SoldierSkill skill;
    public Color cardColor = Color.black;
}

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName = "Upgrade";
    [TextArea] public string description = "";
    public Sprite icon;

    // Check if you can still upgrade stuff
    public virtual bool IsAvailable(UpgradeContext ctx) => true;

    public abstract void Apply(UpgradeContext ctx);
}