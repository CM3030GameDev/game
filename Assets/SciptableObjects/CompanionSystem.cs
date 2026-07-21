using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CompanionSystem", menuName = "Scriptable Objects/CompanionSystem")]
public class CompanionSystem : ScriptableObject
{
    //Current character for player, companion Q, and companion E
    public string playerCharacter = "soldier";
    public string companionQCharacter = "mercenary";
    public string companionECharacter = null;

    //Character's respective healthpoint
    public int soldierHealth = 100;
    public int mercenaryHealth = 100;
    public int swordsmanHealth = 100;

    //Experience point
    public int expPoint = 0;

    //Character level
    public int characterLevel = 1;

    //Movement speed for all characters
    public float characterMS = 7f;

    //Attack speed for all characters
    public float characterAS = 1f;

    //Max health for all characters
    public int maxHealth = 100;

    //Character death state
    public bool playerDead = false;
    public bool companionQDead = false;
    public bool companionEDead = false;
    public bool allDead = false;

    //Companion swap state
    public bool companionSwap = false;

    //Companion swap activated by player
    public void ManualSwap()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !companionQDead)
        {
            string tempCharacter = playerCharacter;
            playerCharacter = companionQCharacter;
            companionQCharacter = tempCharacter;
            companionSwap = true;
        }

        //Only allow companion E swap after act 1
        if(Input.GetKeyDown(KeyCode.E) && companionECharacter != null && !companionEDead)
        {
            string tempCharacter = playerCharacter;
            playerCharacter = companionECharacter;
            companionECharacter = tempCharacter;
            companionSwap = true;
        }
    }

    //Companion swap activated automatically if player character died and there are still companion alive
    public void AutoSwap()
    {
        if(playerDead)
        {
            //Swap to companion Q if companion Q is not dead
            if (!companionQDead)
            {
                playerDead = false;
                companionQDead = true;
                string tempCharacter = playerCharacter;
                playerCharacter = companionQCharacter;
                companionQCharacter = tempCharacter;
                companionSwap = true;
            }
            //Swap to companion E if companion Q is dead and companion E is not dead
            else if (!companionEDead && companionECharacter != null)
            {
                playerDead = false;
                companionEDead = true;
                string tempCharacter = playerCharacter;
                playerCharacter = companionECharacter;
                companionECharacter = tempCharacter;
                companionSwap = true;
            }
            //All characters are dead
            else
            {
                allDead = true;
            }
        }
    }

    //Reset to default
    public void ResetCompanions()
    {
        playerCharacter = "soldier";
        companionQCharacter = "mercenary";
        companionECharacter = null;
        soldierHealth = 100;
        mercenaryHealth = 100;
        swordsmanHealth = 100;
        expPoint = 0;
        characterLevel = 1;
        characterMS = 7f;
        characterAS = 1f;
        maxHealth = 100;
        playerDead = false;
        companionQDead = false;
        companionEDead = false;
        allDead = false;
        companionSwap = false;
    }
}
