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
                    bar.value = Mathf.Clamp(companionSystem.soldierHealth, 0, companionSystem.maxHealth);
                    if(companionSystem.soldierHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
                else if (companionSystem.playerCharacter == "mercenary")
                {
                    bar.value = Mathf.Clamp(companionSystem.mercenaryHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.mercenaryHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
                else
                {
                    bar.value = Mathf.Clamp(companionSystem.swordsmanHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.swordsmanHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
            }
            else if(isCompanionQ)
            {
                if (companionSystem.companionQCharacter == "soldier")
                {
                    bar.value = Mathf.Clamp(companionSystem.soldierHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.soldierHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
                else if (companionSystem.companionQCharacter == "mercenary")
                {
                    bar.value = Mathf.Clamp(companionSystem.mercenaryHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.mercenaryHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
                else
                {
                    bar.value = Mathf.Clamp(companionSystem.swordsmanHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.swordsmanHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
            }
            else if(isCompanionE)
            {
                if (companionSystem.companionECharacter == "soldier")
                {
                    bar.value = Mathf.Clamp(companionSystem.soldierHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.soldierHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
                else if (companionSystem.companionECharacter == "mercenary")
                {
                    bar.value = Mathf.Clamp(companionSystem.mercenaryHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.mercenaryHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
                else
                {
                    bar.value = Mathf.Clamp(companionSystem.swordsmanHealth, 0, companionSystem.maxHealth);
                    if (companionSystem.swordsmanHealth == 0)
                    {
                        companionSystem.playerDead = true;
                    }
                }
            }
        }

        if(isEXP)
        {
            if (companionSystem.expPoint >= 100)
            {
                companionSystem.expPoint = companionSystem.expPoint % 100;
                companionSystem.characterLevel += 1;
            }
            bar.value = companionSystem.expPoint;
        }
    }
}
