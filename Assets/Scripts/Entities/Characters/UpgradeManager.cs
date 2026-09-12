using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Progression progression;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MainWeapon weapon;
    [SerializeField] private SoldierSkill skill;
    [SerializeField] private WeaponSlots slots;
    [SerializeField] private StatLevels statLevels;
    [SerializeField] private UpgradeCardUI cardUI;
    [Tooltip("Carries the picked upgrades between acts. Without it the loadout resets each scene.")]
    [SerializeField] private LoadoutState loadout;

    [Header("Pool")]
    [SerializeField] private List<Upgrade> upgradePool = new List<Upgrade>();

    [System.Serializable]
    public struct MainWeaponStep
    {
        public WeaponTier tier;
        public int requiredLevel;   // 1 for the starting weapon
    }

    [Header("Automatic main weapon tiers (in order, first entry = starting weapon)")]
    [SerializeField] private MainWeaponStep[] mainWeaponTrack;

    public IReadOnlyList<MainWeaponStep> MainWeaponTrack => mainWeaponTrack;

    private UpgradeContext ctx;

    private void Awake()
    {
        ctx = new UpgradeContext { stats = stats, weapon = weapon, skill = skill, slots = slots, statLevels = statLevels };
        BuildStatPartners();

        // Rebuild what the player already owns before applying the tier, so the HUD slots and
        // pips are populated by the time anything reads them.
        RestoreLoadout();
        ApplyMainWeaponTier(loadout != null ? loadout.mainWeaponTier : 0);
    }

    // Replays the picks as bookkeeping only. Upgrade.Apply is deliberately NOT called: the stat
    // effects are already baked into CharacterStats, which persists on its own.
    private void RestoreLoadout()
    {
        if (loadout == null) return;

        foreach (LoadoutState.OwnedWeapon w in loadout.weapons)
        {
            if (w.data == null) continue;

            slots.AcquireOrLevel(w.data);
            SecondaryWeapon sw = slots.Find(w.data);
            while (sw != null && sw.Level < w.level && !sw.IsMaxLevel) sw.LevelUp();
            if (sw != null) slots.SetSlotLevel(sw);
        }

        foreach (LoadoutState.OwnedStat st in loadout.stats)
        {
            if (st.stat == null) continue;

            while (statLevels.GetLevel(st.stat) < st.level) statLevels.Increment(st.stat);
            statLevels.RefreshSlot(st.stat);
        }
    }

    // Snapshot after every pick, so a mid-act scene change keeps the loadout.
    private void SaveLoadout()
    {
        if (loadout == null) return;

        loadout.weapons.Clear();
        foreach (SecondaryWeapon w in slots.Active)
            loadout.weapons.Add(new LoadoutState.OwnedWeapon { data = w.Data, level = w.Level });

        loadout.stats.Clear();
        foreach (var kv in statLevels.All)
            loadout.stats.Add(new LoadoutState.OwnedStat { stat = kv.Key, level = kv.Value });
    }

    private void ApplyMainWeaponTier(int index)
    {
        if (mainWeaponTrack == null || index < 0 || index >= mainWeaponTrack.Length) return;

        WeaponTier tier = mainWeaponTrack[index].tier;
        if (tier == null) return;

        weapon.SetTier(tier);
        slots.SetMainWeaponIcon(tier.icon);
        slots.SetMainWeaponLevel(index + 1);
        if (loadout != null) loadout.mainWeaponTier = index;
    }

    // Derived from the pool so stat cards can name their weapon without storing it twice.
    private void BuildStatPartners()
    {
        foreach (var u in upgradePool)
        {
            if (!(u is SecondaryWeaponUpgrade wu) || wu.weapon == null) continue;

            SecondaryWeaponData d = wu.weapon;
            if (d.combinesWithStat == null || d.combinedResult == null) continue;

            ctx.statPartners[d.combinesWithStat] = d;
        }
    }

    private void OnEnable()
    {
        progression.OnLevelUp += HandleLevelUp;
    }

    private void OnDisable()
    {
        progression.OnLevelUp -= HandleLevelUp;
    }

    private void HandleLevelUp(int newLevel)
    {
        // Main Weapon upgrades automatically at its configured levels
        for (int i = 0; i < mainWeaponTrack.Length; i++)
        {
            if (mainWeaponTrack[i].requiredLevel != newLevel) continue;
            ApplyMainWeaponTier(i);
            break;
        }

        Time.timeScale = 0f; // Pause game when prompted
        List<Upgrade> choices = BuildChoices(newLevel);
        cardUI.Show(choices, ctx, Choose);
    }

    private List<Upgrade> BuildChoices(int level)
    {
        var candidates = new List<Upgrade>();
        foreach (var u in upgradePool)
        {
            if (!u.IsAvailable(ctx)) continue;
            candidates.Add(u);
        }

        var choices = new List<Upgrade>();
        while (choices.Count < 4 && candidates.Count > 0)
        {
            int i = Random.Range(0, candidates.Count);
            choices.Add(candidates[i]);
            candidates.RemoveAt(i);
        }

        return choices;
    }

    private void Choose(Upgrade picked)
    {
        // finally, so a broken upgrade can't leave the game stuck at timeScale 0
        try
        {
            picked.Apply(ctx);
            TryCombineWeapons();
            SaveLoadout();
        }
        finally
        {
            Time.timeScale = 1f; // Resume game
        }
    }

    // A weapon evolves once it and its paired stat are both maxed. Checked after every pick.
    private void TryCombineWeapons()
    {
        if (slots == null || statLevels == null) return;

        foreach (var w in slots.Active)
        {
            SecondaryWeaponData d = w.Data;
            if (d == null || d.isCombinedForm) continue;
            if (d.combinedResult == null || d.combinesWithStat == null) continue;
            if (!w.IsMaxLevel) continue;
            if (!statLevels.IsMaxLevel(d.combinesWithStat)) continue;

            slots.Combine(w, d.combinedResult);
            statLevels.MarkCombined(d.combinesWithStat);   // green the stat pips too
        }
    }
}