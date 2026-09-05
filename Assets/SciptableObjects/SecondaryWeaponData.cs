using UnityEngine;

[System.Serializable]
public class WeaponLevel
{
    public float fireInterval = 2f;
    public int damage = 15;
    public float range = 8f;
    public float valueA;   // per-weapon: stream count / pellets / radius / max mines
    public float valueB;   // per-weapon: spread / slow multiplier
    [TextArea] public string levelDescription;
}

[CreateAssetMenu(menuName = "Scriptable Objects/Secondary Weapon")]
public class SecondaryWeaponData : ScriptableObject
{
    public string weaponName = "Weapon";
    [TextArea] public string description;
    public Sprite icon;
    public GameObject weaponPrefab;
    public WeaponLevel[] levels = new WeaponLevel[3];

    [Header("Combo")]
    public CharacterStatsUpgrade combinesWithStat;
    public SecondaryWeaponData combinedResult;
    [Tooltip("Tick on the combined variants themselves. Gates their special behaviour and stops them re-combining.")]
    public bool isCombinedForm;

    public int MaxLevel => levels.Length;
}