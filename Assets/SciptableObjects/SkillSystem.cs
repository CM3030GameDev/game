using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSystem", menuName = "Scriptable Objects/SkillSystem")]
public class SkillSystem : ScriptableObject
{
    //Store skill names
    public List<string> skillName;
    //Store skill sprites
    public List<Sprite> skillSprite;
    //Dictionary for skill name and skill sprite
    public Dictionary<string, Sprite> skillDict = new Dictionary<string, Sprite>();

    //Store unlocked skills
    public List<string> unlockedSkills = new List<string>();

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        skillDict.Clear();
        //Populate skill dictionary
        for (int i = 0; i < skillName.Count; i++)
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
