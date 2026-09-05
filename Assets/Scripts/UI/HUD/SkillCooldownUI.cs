using UnityEngine;
using UnityEngine.UI;

// Charge readout for one skill slot: a single icon that radially fills in as the skill charges,
// dim while charging and full colour once ready. The owning PlayerSkill pushes its state in each
// frame rather than this pulling it out - assigning the HUD slot from the skill's own Inspector
// avoids a picker where three PlayerSkill components on one GameObject look identical.
public class SkillCooldownUI : MonoBehaviour
{
    [Tooltip("Image Type must be Filled, Fill Method Radial 360.")]
    [SerializeField] private Image icon;

    [SerializeField] private Color readyColor = Color.white;
    [SerializeField] private Color chargingColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    public void SetCharge(float fraction, bool ready)
    {
        if (icon == null) return;

        icon.fillAmount = fraction;
        icon.color = ready ? readyColor : chargingColor;
    }
}
