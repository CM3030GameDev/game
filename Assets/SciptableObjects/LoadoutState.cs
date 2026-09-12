using System.Collections.Generic;
using UnityEngine;

// Everything the player has picked up, kept outside the scene so it survives an act change.
// CharacterStats already persists the raw numbers; this persists what produced them, which the
// scene objects (WeaponSlots, StatLevels, MainWeapon) rebuild themselves from on load.
[CreateAssetMenu(fileName = "LoadoutState", menuName = "Scriptable Objects/LoadoutState")]
public class LoadoutState : ScriptableObject
{
    [System.Serializable]
    public struct OwnedWeapon
    {
        public SecondaryWeaponData data;   // the combined form is its own asset, so this covers evolution
        public int level;
    }

    [System.Serializable]
    public struct OwnedStat
    {
        public CharacterStatsUpgrade stat;
        public int level;
    }

    public List<OwnedWeapon> weapons = new List<OwnedWeapon>();
    public List<OwnedStat> stats = new List<OwnedStat>();
    public int mainWeaponTier;

    public void ResetLoadout()
    {
        weapons.Clear();
        stats.Clear();
        mainWeaponTier = 0;
    }
}
