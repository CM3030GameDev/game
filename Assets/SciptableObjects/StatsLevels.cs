using UnityEngine;
using System.Collections.Generic;

public class StatLevels : MonoBehaviour
{
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
}