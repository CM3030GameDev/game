using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{
    private TMP_Text value;
    [SerializeField] private CharacterStats characterStats;
    [Tooltip("Shown before the number. Trailing space included, e.g. \"Soldier Lvl: \".")]
    [SerializeField] private string prefix = "Soldier Lvl: ";

    private int lastLevel = -1;

    private void Awake()
    {
        value = GetComponent<TMP_Text>();
    }

    // Only rebuild the string when the level actually changed, since this runs every frame.
    private void Update()
    {
        if (characterStats.level == lastLevel) return;

        lastLevel = characterStats.level;
        value.text = prefix + lastLevel;
    }
}
