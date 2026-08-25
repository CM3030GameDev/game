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

    [Header("Pool")]
    [SerializeField] private List<Upgrade> upgradePool = new List<Upgrade>();

    [Header("Automatic main weapon tiers")]
    [SerializeField] private int dualPistolLevel = 5;
    [SerializeField] private WeaponTier dualPistolTier;
    [SerializeField] private int assaultRifleLevel = 10;
    [SerializeField] private WeaponTier assaultRifleTier;

    private UpgradeContext ctx;

    private void Awake()
    {
        ctx = new UpgradeContext { stats = stats, weapon = weapon, skill = skill, slots = slots, statLevels = statLevels };
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
        // Main Weapon upgrades automatically
        if (newLevel == dualPistolLevel && dualPistolTier != null)
        {
            weapon.SetTier(dualPistolTier);
           // slots.SetMainWeaponIcon(dualPistolTier.icon);
        }
        else if (newLevel == assaultRifleLevel && assaultRifleTier != null)
        {
            weapon.SetTier(assaultRifleTier);
           // slots.SetMainWeaponIcon(assaultRifleTier.icon);
        }

        Time.timeScale = 0f; // Pause game when prompted
        List<Upgrade> choices = BuildChoices(newLevel);
        cardUI.Show(choices, Choose);
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
        picked.Apply(ctx);
        Time.timeScale = 1f; // Resume game
    }
}