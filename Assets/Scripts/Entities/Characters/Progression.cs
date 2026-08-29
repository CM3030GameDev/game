using System;
using UnityEngine;

// Progression to track level ups
public class Progression : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private int expPerLevel = 100;
    [SerializeField] private int maxLevel = 25;

    public event Action<int> OnLevelUp;   // Event to catch level ups

    public int ExpPerLevel => expPerLevel;
    public int MaxLevel => maxLevel;

    private void Update()
    {
        // To handle multiple level ups in one frame
        while (characterStats.expPoint >= expPerLevel && characterStats.level < maxLevel)
        {
            characterStats.expPoint -= expPerLevel;
            characterStats.level += 1;
            OnLevelUp?.Invoke(characterStats.level);
        }
        // Stop accumulating exp past a full exp bar when max level is reached by the player
        if (characterStats.level >= maxLevel && characterStats.expPoint > expPerLevel)
            characterStats.expPoint = expPerLevel;
    }
}