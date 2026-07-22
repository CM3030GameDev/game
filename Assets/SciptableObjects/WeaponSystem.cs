using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSystem", menuName = "Scriptable Objects/WeaponSystem")]
public class WeaponSystem : ScriptableObject, ISerializationCallbackReceiver
{
    //Store character name
    public List<string> characterName;
    //Store character main weapon sprite
    public List<Sprite> weaponSprite;
    //Store skill names
    public List<string> skillName;
    //Store skill sprites
    public List<Sprite> skillSprite;
    //Dictionary for character name and main weapon sprite
    public Dictionary<string, Sprite> weaponDict = new Dictionary<string, Sprite>();
    //Dictionary for skill name and skill sprite
    public Dictionary<string, Sprite> skillDict = new Dictionary<string, Sprite>();

    //Store unlocked skills
    public List<string> unlockedSkills = new List<string>();

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        weaponDict.Clear();
        skillDict.Clear();
        //Populate weapon dictionary
        for (int i = 0; i < characterName.Count; i++)
        {
            weaponDict.Add(characterName[i], weaponSprite[i]);
        }
        //Populate skill dictionary
        for(int i = 0; i < skillName.Count; i++)
        {
            skillDict.Add(skillName[i], skillSprite[i]);
        }
    }

    //Reset to default
    public void ResetSkills()
    {
        unlockedSkills.Clear();
    }
}
