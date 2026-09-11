using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Progression progression;
    [Tooltip("Optional \"0 / 100\" readout centred in the bar.")]
    [SerializeField] private TMP_Text valueText;

    private int lastExp = -1;
    private int lastPerLevel = -1;

    private void Update()
    {
        fillImage.fillAmount = (float)characterStats.expPoint / progression.ExpPerLevel;

        // Only rebuild the string when a number actually changed, since this runs every frame.
        int perLevel = progression.ExpPerLevel;
        if (valueText != null && (characterStats.expPoint != lastExp || perLevel != lastPerLevel))
        {
            lastExp = characterStats.expPoint;
            lastPerLevel = perLevel;
            valueText.text = lastExp + " / " + lastPerLevel;
        }
    }
}
