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

    [Header("Guaranteed Main Weapon Tiers")]
    [SerializeField] private int dualPistolLevel = 5;
    [SerializeField] private WeaponTier dualPistolTier;
    [SerializeField] private int assaultRifleLevel = 10;
    [SerializeField] private WeaponTier assaultRifleTier;

    [Header("Weapon Slots")]
    [SerializeField] private WeaponSlots slots;

    private readonly HashSet<Upgrade> taken = new HashSet<Upgrade>();

    private UpgradeContext ctx;

    private void Awake()
    {
        ctx = new UpgradeContext { stats = stats, weapon = weapon, skill = skill, slots = slots };
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
        if (newLevel == dualPistolLevel && dualPistolTier != null)
            weapon.SetTier(dualPistolTier);
        else if (newLevel == assaultRifleLevel && assaultRifleTier != null)
            weapon.SetTier(assaultRifleTier);

        List<Upgrade> choices = BuildChoices(newLevel);
        cardUI.Show(choices, Choose);
        Time.timeScale = 0f;
    }

    private List<Upgrade> BuildChoices(int level)
    {
        var choices = new List<Upgrade>();

        // Fill slots randomly from available upgrades
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
        Time.timeScale = 1f;
    }
}