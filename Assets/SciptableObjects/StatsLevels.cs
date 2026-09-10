using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatLevels : MonoBehaviour
{
    [SerializeField] private int maxSlots = 5;   // max distinct stats active at once, out of however many CharacterStatsUpgrade types exist
    [SerializeField] private Image[] statSlotImages;
    [SerializeField] private LevelPips[] statSlotPips;   // same indexing as statSlotImages

    private readonly Dictionary<CharacterStatsUpgrade, int> levels
        = new Dictionary<CharacterStatsUpgrade, int>();

    // Slots are assigned in acquisition order, like WeaponSlots (first stat picked gets slot 0)
    private readonly List<CharacterStatsUpgrade> order = new List<CharacterStatsUpgrade>();

    public bool IsFull => order.Count >= maxSlots;

    public int GetLevel(CharacterStatsUpgrade u)
        => levels.TryGetValue(u, out int v) ? v : 0;

    public bool IsMaxLevel(CharacterStatsUpgrade u)
        => GetLevel(u) >= u.MaxLevel;

    public bool CanOffer(CharacterStatsUpgrade u)
    {
        if (levels.ContainsKey(u)) return !IsMaxLevel(u);
        return !IsFull;
    }

    public int Increment(CharacterStatsUpgrade u)
    {
        if (!levels.ContainsKey(u)) order.Add(u);
        int next = GetLevel(u) + 1;
        levels[u] = next;
        return next;
    }

    // For the HUD stat slot row (Top Left Cluster on HUD in Hiearchy)
    public IEnumerable<KeyValuePair<CharacterStatsUpgrade, int>> All => levels;

    // Called when this stat's paired weapon evolves, since both halves were maxed to get here.
    public void MarkCombined(CharacterStatsUpgrade u)
    {
        int i = order.IndexOf(u);
        if (i < 0 || i >= statSlotPips.Length || statSlotPips[i] == null) return;
        statSlotPips[i].SetCombined(true);
    }

    public void RefreshSlot(CharacterStatsUpgrade u)
    {
        int i = order.IndexOf(u);
        if (i < 0 || i >= statSlotImages.Length) return;
        int level = GetLevel(u);
        statSlotImages[i].sprite = u.icon;
        statSlotImages[i].color = Color.white;

        if (i < statSlotPips.Length && statSlotPips[i] != null)
            statSlotPips[i].SetLevel(level);
    }
}