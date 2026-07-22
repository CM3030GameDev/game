using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isCompanionQ;
    [SerializeField] private bool isCompanionE;
    [SerializeField] private bool isHP;
    [SerializeField] private bool isEXP;
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private Slider bar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isHP)
        {
            if (isPlayer)
            {
                if (companionSystem.playerCharacter == "soldier")
                {
                    if(companionSystem.soldierHealth > 0)
                    {
                        bar.value = companionSystem.soldierHealth;
                    }
                    else
                    {
                        bar.value = 0;
                        companionSystem.playerDead = true;
                    }
                }
                else if (companionSystem.playerCharacter == "mercenary")
                {
                    if (companionSystem.mercenaryHealth > 0)
                    {
                        bar.value = companionSystem.mercenaryHealth;
                    }
                    else
                    {
                        bar.value = 0;
                        companionSystem.playerDead = true;
                    }
                }
                else
                {
                    if (companionSystem.swordsmanHealth > 0)
                    {
                        bar.value = companionSystem.swordsmanHealth;
                    }
                    else
                    {
                        bar.value = 0;
                        companionSystem.playerDead = true;
                    }
                }
            }
            else if(isCompanionQ)
            {
                if (companionSystem.companionQCharacter == "soldier")
                {
                    if (companionSystem.soldierHealth > 0)
                    {
                        bar.value = companionSystem.soldierHealth;
                    }
                    else
                    {
                        bar.value = 0;
                    }
                }
                else if (companionSystem.companionQCharacter == "mercenary")
                {
                    if (companionSystem.mercenaryHealth > 0)
                    {
                        bar.value = companionSystem.mercenaryHealth;
                    }
                    else
                    {
                        bar.value = 0;
                    }
                }
                else
                {
                    if (companionSystem.swordsmanHealth > 0)
                    {
                        bar.value = companionSystem.swordsmanHealth;
                    }
                    else
                    {
                        bar.value = 0;
                    }
                }
            }
            else if(isCompanionE)
            {
                if (companionSystem.companionECharacter == "soldier")
                {
                    if (companionSystem.soldierHealth > 0)
                    {
                        bar.value = companionSystem.soldierHealth;
                    }
                    else
                    {
                        bar.value = 0;
                    }
                }
                else if (companionSystem.companionECharacter == "mercenary")
                {
                    if (companionSystem.mercenaryHealth > 0)
                    {
                        bar.value = companionSystem.mercenaryHealth;
                    }
                    else
                    {
                        bar.value = 0;
                    }
                }
                else
                {
                    if (companionSystem.swordsmanHealth > 0)
                    {
                        bar.value = companionSystem.swordsmanHealth;
                    }
                    else
                    {
                        bar.value = 0;
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
            bar.value = companionSystem.expPoint;
        }
    }
}
