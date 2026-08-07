using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Progression progression;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private MainWeapon weapon;
    [SerializeField] private SoldierSkill skill;
    [SerializeField] private UpgradeCardUI cardUI;

    [Header("Pool")]
    [SerializeField] private List<Upgrade> upgradePool = new List<Upgrade>();

    [Header("Guaranteed weapon tiers")]
    [SerializeField] private int dualPistolLevel = 5;
    [SerializeField] private WeaponTierUpgrade dualPistolUpgrade;
    [SerializeField] private int assaultRifleLevel = 10;
    [SerializeField] private WeaponTierUpgrade assaultRifleUpgrade;

    private readonly HashSet<Upgrade> taken = new HashSet<Upgrade>();

    private UpgradeContext ctx;

    private void Awake()
    {
        ctx = new UpgradeContext { stats = stats, weapon = weapon, skill = skill };
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
        List<Upgrade> choices = BuildChoices(newLevel);
        Time.timeScale = 0f; // Pause game when prompted
        cardUI.Show(choices, Choose);
    }

    private List<Upgrade> BuildChoices(int level)
    {
        var choices = new List<Upgrade>();

        // Guaranteed weapon tier at set levels
        if (level == dualPistolLevel && dualPistolUpgrade != null)
            choices.Add(dualPistolUpgrade);
        else if (level == assaultRifleLevel && assaultRifleUpgrade != null)
            choices.Add(assaultRifleUpgrade);

        // Fill remaining slots randomly from available upgrades
        var candidates = new List<Upgrade>();
        foreach (var u in upgradePool)
        {
            if (taken.Contains(u)) continue;
            if (!u.IsAvailable(ctx)) continue;
            if (choices.Contains(u)) continue;
            candidates.Add(u);
        }

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
        picked.Apply(ctx);

        // Weapon tiers and one-off upgrades shouldn't reappear.
        // Stat upgrades are repeatable (temp)
        if (!(picked is CharacterStatsUpgrade))
            taken.Add(picked);

        Time.timeScale = 1f; // Resume game
    }
}