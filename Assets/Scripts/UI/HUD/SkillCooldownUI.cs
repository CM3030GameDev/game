using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private SoldierSkill skill;
    [SerializeField] private Image fillImage; // Image Type = Filled, Fill Method = Radial 360

    private void Update()
    {
        fillImage.fillAmount = skill.ChargeFraction;
    }
}
