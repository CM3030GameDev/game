using System;
using UnityEngine;

// Progression to track level ups
public class Progression : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [Tooltip("Exp needed for the first level up.")]
    [SerializeField] private int baseExp = 10;
    [Tooltip("Added to the cost of every level after the first, so upgrades start fast and " +
             "then have to be earned.")]
    [SerializeField] private int expGrowth = 10;
    [SerializeField] private int maxLevel = 25;

    public event Action<int> OnLevelUp;   // Event to catch level ups

    // Cost of the level the player is currently working through, not a fixed number.
    // The exp bar and its readout both use this, so they follow the curve for free.
    public int ExpPerLevel => baseExp + expGrowth * Mathf.Max(0, characterStats.level - 1);
    public int MaxLevel => maxLevel;

    private void Update()
    {
        // To handle multiple level ups in one frame
        while (characterStats.expPoint >= ExpPerLevel && characterStats.level < maxLevel)
        {
            characterStats.expPoint -= ExpPerLevel;
            characterStats.level += 1;
            OnLevelUp?.Invoke(characterStats.level);
        }
        // Stop accumulating exp past a full exp bar when max level is reached by the player
        if (characterStats.level >= maxLevel && characterStats.expPoint > ExpPerLevel)
            characterStats.expPoint = ExpPerLevel;
    }
}