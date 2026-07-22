using UnityEngine;
using UnityEngine.UI;

public class CharacterSprite : MonoBehaviour
{
    [SerializeField] private Image imageSprite;
    [SerializeField] private Sprite soldierSprite;
    [SerializeField] private Sprite mercenarySprite;
    [SerializeField] private Sprite swordsmanSprite;
    [SerializeField] private bool isCompanionQ;
    [SerializeField] private bool isCompanionE;
    [SerializeField] private CompanionSystem companionSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCompanionQ)
        {
            if (companionSystem.companionQCharacter == "soldier")
            {
                imageSprite.sprite = soldierSprite;
            }
            else if (companionSystem.companionQCharacter == "mercenary")
            {
                imageSprite.sprite = mercenarySprite;
            }
            else if (companionSystem.companionQCharacter == "swordsman")
            {
                imageSprite.sprite = swordsmanSprite;
            }
        }

        if (isCompanionE)
        {
            if (companionSystem.companionECharacter == "soldier")
            {
                imageSprite.sprite = soldierSprite;
            }
            else if (companionSystem.companionECharacter == "mercenary")
            {
                imageSprite.sprite = mercenarySprite;
            }
            else if (companionSystem.companionECharacter == "swordsman")
            {
                imageSprite.sprite = swordsmanSprite;
            }
        }
    }
}
