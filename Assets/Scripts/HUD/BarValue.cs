using UnityEngine;
using TMPro;

public class BarValue : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isCompanionQ;
    [SerializeField] private bool isCompanionE;
    [SerializeField] private bool isHP;
    [SerializeField] private bool isEXP;
    [SerializeField] private bool isBGM;
    [SerializeField] private bool isSFX;
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private TMP_Text value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHP)
        {
            if (isPlayer)
            {
                if (companionSystem.playerCharacter == "soldier")
                {
                    if (companionSystem.soldierHealth >= 0)
                    {
                        value.text = $"{companionSystem.soldierHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
                else if (companionSystem.playerCharacter == "mercenary")
                {
                    if (companionSystem.mercenaryHealth >= 0)
                    {
                        value.text = $"{companionSystem.mercenaryHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
                else
                {
                    if (companionSystem.swordsmanHealth >= 0)
                    {
                        value.text = $"{companionSystem.swordsmanHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
            }

            if (isCompanionQ)
            {
                if (companionSystem.companionQCharacter == "soldier")
                {
                    if (companionSystem.soldierHealth >= 0)
                    {
                        value.text = $"{companionSystem.soldierHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
                else if (companionSystem.companionQCharacter == "mercenary")
                {
                    if (companionSystem.mercenaryHealth >= 0)
                    {
                        value.text = $"{companionSystem.mercenaryHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
                else
                {
                    if (companionSystem.swordsmanHealth >= 0)
                    {
                        value.text = $"{companionSystem.swordsmanHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
            }

            if (isCompanionE)
            {
                if (companionSystem.companionECharacter == "soldier")
                {
                    if (companionSystem.soldierHealth >= 0)
                    {
                        value.text = $"{companionSystem.soldierHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
                else if (companionSystem.companionECharacter == "mercenary")
                {
                    if (companionSystem.mercenaryHealth >= 0)
                    {
                        value.text = $"{companionSystem.mercenaryHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
                else
                {
                    if (companionSystem.swordsmanHealth >= 0)
                    {
                        value.text = $"{companionSystem.swordsmanHealth}/{companionSystem.maxHealth}";
                    }
                    else
                    {
                        value.text = $"0/{companionSystem.maxHealth}";
                    }
                }
            }
        }

        if(isEXP)
        {
            if (companionSystem.expPoint >= 100)
            {
                companionSystem.expPoint = companionSystem.expPoint % 100;
            }
            value.text = $"{companionSystem.expPoint}/100";
        }
    }
}
