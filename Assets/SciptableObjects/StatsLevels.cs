using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatLevels : MonoBehaviour
{
    [SerializeField] private Image[] statSlotImages;
    [SerializeField] private CharacterStatsUpgrade[] statOrder;

    private readonly Dictionary<CharacterStatsUpgrade, int> levels
        = new Dictionary<CharacterStatsUpgrade, int>();

    public int GetLevel(CharacterStatsUpgrade u)
        => levels.TryGetValue(u, out int v) ? v : 0;

    public bool IsMaxLevel(CharacterStatsUpgrade u)
        => GetLevel(u) >= u.maxLevel;

    public int Increment(CharacterStatsUpgrade u)
    {
        int next = GetLevel(u) + 1;
        levels[u] = next;
        return next;
    }

    // For the HUD stat slot row (Top Left Cluster on HUD in Hiearchy)
    public IEnumerable<KeyValuePair<CharacterStatsUpgrade, int>> All => levels;

    public void RefreshSlot(CharacterStatsUpgrade u)
    {
        int i = System.Array.IndexOf(statOrder, u);
        if (i < 0) return;
        int level = GetLevel(u);
        statSlotImages[i].sprite = u.iconPerLevel[level - 1]; // if you have per-level star art
    }
}