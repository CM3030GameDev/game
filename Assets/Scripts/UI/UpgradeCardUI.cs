using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button[] cardButtons = new Button[4];
    [SerializeField] private TMP_Text[] cardNames = new TMP_Text[4];
    [SerializeField] private TMP_Text[] cardDescriptions = new TMP_Text[4];
    [SerializeField] private Image[] cardIcons = new Image[4];
    [Header("Combo row")]
    [SerializeField] private Image[] cardComboIcons = new Image[4];
    [SerializeField] private TMP_Text[] cardComboTexts = new TMP_Text[4];

    [Header("Stats Panel")]
    [SerializeField] private TMP_Text statsLevelText;            // "LEVEL n" header, centred (optional)
    [SerializeField] private TMP_Text statsText;                 // labels column, left aligned
    [SerializeField] private TMP_Text statsValues;               // values column, right aligned (optional)
    [SerializeField] private CharacterStats stats;

    [Header("Stat row icons")]
    [SerializeField] private string healthIcon;
    [SerializeField] private string moveSpeedIcon;
    [SerializeField] private string fireRateIcon;
    [SerializeField] private string damageIcon;
    [SerializeField] private string pickupRangeIcon;
    [SerializeField] private string healthRegenIcon;

    [Header("Combination highlight")]
    [Tooltip("Card tint when picking it would evolve a weapon. Default is 00FF33.")]
    [SerializeField] private Color combineColor = new Color(0f, 1f, 0.2f, 1f);

    [Header("Focus")]
    [SerializeField] private CanvasGroup persistentHud;   // HP bar, EXP bar, weapon/stat pips etc.
    [SerializeField] private float dimmedAlpha = 0.3f;

    private Action<Upgrade> onChosen;

    // Authored card colours, captured before anything tints them.
    // ColorTint multiplies against Image.color, so tinting the Image directly survives hover.
    private Color[] baseColors;

    private void Awake()
    {
        panel.SetActive(false);

        baseColors = new Color[cardButtons.Length];
        for (int i = 0; i < cardButtons.Length; i++)
        {
            Graphic g = cardButtons[i] != null ? cardButtons[i].targetGraphic : null;
            baseColors[i] = g != null ? g.color : Color.white;
        }
    }

    public void Show(List<Upgrade> choices, UpgradeContext ctx, Action<Upgrade> callback)
    {
        onChosen = callback;
        panel.SetActive(true);
        if (persistentHud != null) persistentHud.alpha = dimmedAlpha;
        RefreshStats();

        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                Upgrade u = choices[i];
                cardButtons[i].gameObject.SetActive(true);
                cardNames[i].text = u.upgradeName;
                SetCardText(i, u.GetDescription(ctx), u.GetComboText(ctx));

                Sprite cardIcon = u.GetIcon(ctx);
                if (cardIcons[i] != null && cardIcon != null)
                {
                    cardIcons[i].sprite = cardIcon;
                    cardIcons[i].preserveAspect = true;
                }

                SetComboIcon(i, u.GetComboIcon(ctx));
                SetCombineHighlight(i, u.OwnsComboPartner(ctx));

                cardButtons[i].onClick.RemoveAllListeners();
                cardButtons[i].onClick.AddListener(() => Pick(u));
            }
            else
            {
                cardButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetCardText(int i, string description, string combo)
    {
        bool hasComboSlot = cardComboTexts != null && i < cardComboTexts.Length && cardComboTexts[i] != null;

        if (hasComboSlot)
        {
            cardDescriptions[i].text = description;
            cardComboTexts[i].text = combo ?? string.Empty;
            cardComboTexts[i].gameObject.SetActive(combo != null);
        }
        else
        {
            cardDescriptions[i].text = combo == null ? description : description + "\n\n" + combo;
        }
    }

    // Set on every Show, so a card that lit up last level goes back to normal.
    private void SetCombineHighlight(int i, bool combining)
    {
        if (baseColors == null || i >= cardButtons.Length || cardButtons[i] == null) return;

        Graphic g = cardButtons[i].targetGraphic;
        if (g != null) g.color = combining ? combineColor : baseColors[i];
    }

    private void SetComboIcon(int i, Sprite sprite)
    {
        if (cardComboIcons == null || i >= cardComboIcons.Length || cardComboIcons[i] == null) return;

        cardComboIcons[i].sprite = sprite;
        cardComboIcons[i].preserveAspect = true;
        cardComboIcons[i].gameObject.SetActive(sprite != null);
    }

    private void RefreshStats()
    {
        if (statsText == null || stats == null) return;

        string[] icons  = { healthIcon, moveSpeedIcon, fireRateIcon, damageIcon, pickupRangeIcon, healthRegenIcon };
        string[] labels = { "Health", "Move Speed", "Fire Rate", "Damage", "Pickup Range", "Health Regen" };
        string[] values =
        {
            $"{stats.health} / {stats.maxHealth}",
            $"{stats.moveSpeed:F0}",
            $"{stats.attackSpeed:F1}s",
            $"+{stats.damage:F0}",
            $"{stats.pickupRadius:F0}",
            $"{stats.healthRegen:F0}/s",
        };

        // Header sits in its own object when wired, so it stays centred while rows align.
        string left, right;
        if (statsLevelText != null)
        {
            statsLevelText.text = $"LEVEL {stats.level}";
            left = string.Empty;
            right = string.Empty;
        }
        else
        {
            left = $"<b>LEVEL {stats.level}</b>\n\n";
            right = "\n\n";   // blank lines matching the header, so rows line up
        }

        // Two text objects put labels left and values right. Falls back to one column.
        if (statsValues != null)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                left += Prefix(icons[i]) + labels[i] + "\n";
                right += values[i] + "\n";
            }
            statsText.text = left;
            statsValues.text = right;
        }
        else
        {
            for (int i = 0; i < labels.Length; i++)
                left += Prefix(icons[i]) + labels[i] + "<pos=62%>" + values[i] + "\n";
            statsText.text = left;
        }
    }

    private static string Prefix(string icon)
        => string.IsNullOrEmpty(icon) ? string.Empty : icon + " ";

    private void Pick(Upgrade u)
    {
        panel.SetActive(false);
        if (persistentHud != null) persistentHud.alpha = 1f;
        onChosen?.Invoke(u);
    }
}