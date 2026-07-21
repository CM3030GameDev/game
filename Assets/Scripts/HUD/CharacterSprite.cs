using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour
{
    [SerializeField] private Image imageSprite;
    [SerializeField] private Sprite soldierSprite;
    [SerializeField] private Sprite mercenarySprite;
    [SerializeField] private Sprite swordsmanSprite;
    [SerializeField] private bool isPlayer;
    [SerializeField] private bool isCompanionQ;
    [SerializeField] private bool isCompanionE;
    [SerializeField] private CompanionSystem cs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayer)
        {
            if(cs.playerCharacter == "soldier")
            {
                imageSprite.sprite = soldierSprite;
            }
            else if (cs.playerCharacter == "mercenary")
            {
                imageSprite.sprite = mercenarySprite;
            }
            else if(cs.playerCharacter == "swordsman")
            {
                imageSprite.sprite = swordsmanSprite;
            }
        }

        if (isCompanionQ)
        {
            if (cs.companionQCharacter == "soldier")
            {
                imageSprite.sprite = soldierSprite;
            }
            else if (cs.companionQCharacter == "mercenary")
            {
                imageSprite.sprite = mercenarySprite;
            }
            else if (cs.companionQCharacter == "swordsman")
            {
                imageSprite.sprite = swordsmanSprite;
            }
        }

        if (isCompanionE)
        {
            if (cs.companionECharacter == "soldier")
            {
                imageSprite.sprite = soldierSprite;
            }
            else if (cs.companionECharacter == "mercenary")
            {
                imageSprite.sprite = mercenarySprite;
            }
            else if (cs.companionECharacter == "swordsman")
            {
                imageSprite.sprite = swordsmanSprite;
            }
        }
    }
}
