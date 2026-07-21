using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private Sprite soldierSkill;
    [SerializeField] private Sprite mercenarySkill;
    [SerializeField] private Sprite swordsmanSkill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(companionSystem.playerCharacter == "soldier")
        {
            image.sprite = soldierSkill;
        }
        else if (companionSystem.playerCharacter == "mercenary")
        {
            image.sprite = mercenarySkill;
        }
        else
        {
            image.sprite = swordsmanSkill;
        }
    }
}
