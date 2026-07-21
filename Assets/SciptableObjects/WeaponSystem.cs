using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSystem", menuName = "Scriptable Objects/WeaponSystem")]
public class WeaponSystem : ScriptableObject, ISerializationCallbackReceiver
{
    //Store weapon names
    public List<string> weaponName;
    //Store original weapon sprites
    public List<Sprite> weaponSprite;
    //Store equipped weapon sprites
    public List<Sprite> equippedSprite;
    //Store weapon name and original weapon sprite
    public Dictionary<string, Sprite> originalDict = new Dictionary<string, Sprite>();
    //Store weapon name and equipped weapon sprite
    public Dictionary<string, Sprite> equippedDict = new Dictionary<string, Sprite>();
    //Store current equipped weapon name
    public string currentWeapon = "pistol";

    //Store unlocked weapons
    public List<string> unlockedWeapons = new List<string>();

    public void OnBeforeSerialize()
    {

    }

    public void OnAfterDeserialize()
    {
        originalDict.Clear();
        equippedDict.Clear();
        for (int i = 0; i < weaponName.Count; i++)
        {
            originalDict.Add(weaponName[i], weaponSprite[i]);
            equippedDict.Add(weaponName[i], equippedSprite[i]);
        }
    }

    //Reset to default
    public void ResetWeapons()
    {
        unlockedWeapons.Clear();
        currentWeapon = "pistol";
    }
}
