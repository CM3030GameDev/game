using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{
    [SerializeField] private GameObject weapon1;
    [SerializeField] private GameObject weapon2;
    [SerializeField] private GameObject weapon3;
    [SerializeField] private GameObject weapon4;
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    [SerializeField] private Image image4;
    [SerializeField] private RectTransform highlight;
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private WeaponAttack weaponAttack;
    private Vector3 originalPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalPos = highlight.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Character is currently a soldier
        if (companionSystem.playerCharacter == "soldier")
        {
            image1.sprite = weaponSystem.originalDict["pistol"];
            if (Input.GetKeyDown(KeyCode.Alpha1) || weaponSystem.currentWeapon == "flamethrower" || weaponSystem.currentWeapon == "sword")
            {
                highlight.localPosition = originalPos;
                weaponSystem.currentWeapon = "pistol";
            }
        }
        //Character is currently a mercenary
        else if (companionSystem.playerCharacter == "mercenary")
        {
            image1.sprite = weaponSystem.originalDict["flamethrower"];
            if (Input.GetKeyDown(KeyCode.Alpha1) || weaponSystem.currentWeapon == "pistol" || weaponSystem.currentWeapon == "sword")
            {
                highlight.localPosition = originalPos;
                weaponSystem.currentWeapon = "flamethrower";
            }
        }
        //Character is currently a swordsman
        else
        {
            image1.sprite = weaponSystem.originalDict["sword"];
            if (Input.GetKeyDown(KeyCode.Alpha1) || weaponSystem.currentWeapon == "pistol" || weaponSystem.currentWeapon == "flamethrower")
            {
                highlight.localPosition = originalPos;
                weaponSystem.currentWeapon = "sword";
            }
        }

        //Check if unlockable weapon can be shown in HUD
        if(weaponSystem.unlockedWeapons.Count > 0)
        {
            image2.sprite = weaponSystem.originalDict[weaponSystem.unlockedWeapons[0]];
            weapon2.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                highlight.localPosition = originalPos + new Vector3(150, 0, 0);
                weaponSystem.currentWeapon = weaponSystem.unlockedWeapons[0];
                //Auto attack turned off when equipping unlockable weapons
                weaponAttack.autoAttack = false;
            }
        }
        
        if(weaponSystem.unlockedWeapons.Count > 1)
        {
            image3.sprite = weaponSystem.originalDict[weaponSystem.unlockedWeapons[1]];
            weapon3.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                highlight.localPosition = originalPos + new Vector3(150 * 2, 0, 0);
                weaponSystem.currentWeapon = weaponSystem.unlockedWeapons[1];
                //Auto attack turned off when equipping unlockable weapons
                weaponAttack.autoAttack = false;
            }
        }
        
        if (weaponSystem.unlockedWeapons.Count > 2)
        {
            image4.sprite = weaponSystem.originalDict[weaponSystem.unlockedWeapons[2]];
            weapon4.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                highlight.localPosition = originalPos + new Vector3(150 * 3, 0, 0);
                weaponSystem.currentWeapon = weaponSystem.unlockedWeapons[2];
                //Auto attack turned off when using unlockable weapons
                weaponAttack.autoAttack = false;
            }
        }
    }
}
