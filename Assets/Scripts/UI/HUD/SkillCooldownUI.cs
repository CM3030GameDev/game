using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Charge readout for one skill slot: a single icon that radially fills in as the skill charges,
// dim while charging and full colour once ready. The owning PlayerSkill pushes its state in each
// frame rather than this pulling it out - assigning the HUD slot from the skill's own Inspector
// avoids a picker where three PlayerSkill components on one GameObject look identical.
public class SkillCooldownUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Image Type must be Filled, Fill Method Radial 360.")]
    [SerializeField] private Image icon;

    [SerializeField] private Color readyColor = Color.white;
    [SerializeField] private Color chargingColor = new Color(0.6f, 0.6f, 0.6f, 1f);

    [Tooltip("Shown when the player hovers this slot. Say what the skill does and which key casts it.")]
    [TextArea(2, 5)][SerializeField] private string tooltip;

    public void SetCharge(float fraction, bool ready)
    {
        if (icon == null) return;

        icon.fillAmount = fraction;
        icon.color = ready ? readyColor : chargingColor;
    }

    public void OnPointerEnter(PointerEventData eventData) => TooltipPanel.ShowFor(tooltip, (RectTransform)transform);

    public void OnPointerExit(PointerEventData eventData) => TooltipPanel.HideFor((RectTransform)transform);

    // The Guard slot hides itself when unavailable, which skips OnPointerExit if it was hovered.
    private void OnDisable() => TooltipPanel.HideFor((RectTransform)transform);
}
