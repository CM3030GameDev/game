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

    private Action<Upgrade> onChosen;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(List<Upgrade> choices, Action<Upgrade> callback)
    {
        onChosen = callback;
        panel.SetActive(true);
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
            $"HASTE   {stats.attackSpeed:F2}";
    }

    private void Pick(Upgrade u)
    {
        panel.SetActive(false);
        onChosen?.Invoke(u);
    }
}