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

    [Header("Stats Panel")]
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private CharacterStats stats;

    [Header("Focus")]
    [SerializeField] private CanvasGroup persistentHud;   // HP bar, EXP bar, weapon/stat pips etc.
    [SerializeField] private float dimmedAlpha = 0.3f;

    private Action<Upgrade> onChosen;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(List<Upgrade> choices, Action<Upgrade> callback)
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
                cardDescriptions[i].text = u.description;

                if (cardIcons[i] != null && u.icon != null)
                    cardIcons[i].sprite = u.icon;

                cardButtons[i].onClick.RemoveAllListeners();
                cardButtons[i].onClick.AddListener(() => Pick(u));
            }
            else
            {
                cardButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshStats()
    {
        if (statsText == null || stats == null) return;

        statsText.text =
            $"LEVEL   {stats.level}\n" +
            $"HP      {stats.health} / {stats.maxHealth}\n" +
            $"SPEED   {stats.moveSpeed:F1}\n" +
            $"HASTE   {stats.attackSpeed:F2}\n" +
            $"DAMAGE  +{stats.damage:F0}\n" +
            $"RANGE   {stats.pickupRadius:F1}\n" +
            $"REGEN   {stats.healthRegen:F1}/s";
    }

    private void Pick(Upgrade u)
    {
        panel.SetActive(false);
        if (persistentHud != null) persistentHud.alpha = 1f;
        onChosen?.Invoke(u);
    }
}