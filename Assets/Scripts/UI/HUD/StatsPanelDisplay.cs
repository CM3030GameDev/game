using UnityEngine;
using TMPro;

public class StatsPanelDisplay : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;
    private TMP_Text text;

    private void Awake() => text = GetComponent<TMP_Text>();

    private void Update()
    {
        text.text =
            $"Max Health: {stats.maxHealth}\n" +
            $"Move Speed: {stats.moveSpeed:F1}\n" +
            $"Attack Speed: {stats.attackSpeed:F2}\n" +
            $"Damage: +{stats.damage:F0}\n" +
            $"Pickup Radius: {stats.pickupRadius:F1}\n" +
            $"Health Regen: {stats.healthRegen:F1}/s";
    }
}
