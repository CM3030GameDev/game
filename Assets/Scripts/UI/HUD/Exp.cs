using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Progression progression;

    private void Update()
    {
        fillImage.fillAmount = (float)characterStats.expPoint / progression.ExpPerLevel;
    }
}
