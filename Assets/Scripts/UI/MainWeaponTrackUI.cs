using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Shows the automatic main-weapon progression as a row of boxes with their unlock level above
public class MainWeaponTrackUI : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private CharacterStats stats;

    [Header("One entry per tier")]
    [SerializeField] private Image[] tierIcons;
    [SerializeField] private TMP_Text[] tierLevelLabels;

    [Header("Tint")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = new Color(1f, 1f, 1f, 0.55f);   // dimmed but still legible

    private void OnEnable() => Refresh();

    public void Refresh()
    {
        if (upgradeManager == null || stats == null) return;

        var track = upgradeManager.MainWeaponTrack;
        if (track == null) return;

        for (int i = 0; i < tierIcons.Length; i++)
        {
            bool inTrack = i < track.Count && track[i].tier != null;

            if (tierIcons[i] != null)
            {
                tierIcons[i].gameObject.SetActive(inTrack);
                if (inTrack)
                {
                    tierIcons[i].sprite = track[i].tier.icon;
                    tierIcons[i].preserveAspect = true;
                    tierIcons[i].color = stats.level >= track[i].requiredLevel ? unlockedColor : lockedColor;
                }
            }

            if (i < tierLevelLabels.Length && tierLevelLabels[i] != null)
            {
                tierLevelLabels[i].gameObject.SetActive(inTrack);
                if (inTrack)
                {
                    int required = track[i].requiredLevel;
                    tierLevelLabels[i].text = required <= 1 ? "Start" : $"Lv {required}";
                    tierLevelLabels[i].color = stats.level >= required ? unlockedColor : lockedColor;
                }
            }
        }
    }
}
