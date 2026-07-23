using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject shoot;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject swing;
    [SerializeField] private CompanionSystem companionSystem;
    private Camera mainCamera;
    public bool autoAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = character.transform.position - new Vector3(0, 0.405f, 0);
        transform.rotation = character.transform.rotation;
        mainCamera = Camera.main;
        autoAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Follow relative to character's position
        transform.position = character.transform.position - new Vector3(0, 0.405f, 0);

        //Get mouse position in screen pixels
        Vector3 screenPosition = Input.mousePosition;

        //Convert screen pixels to 3D world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        //Convert 3D world coordinate to 2D 
        worldPosition.z = 0f;

        //Direction vector from player to mouse
        Vector2 direction = worldPosition - transform.position;

        //Normalize direction vector from player to mouse
        Vector2 normalizedDirection = direction.normalized;

        //Convert angle from radian to degree
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Rotate according to current mouse position on screen
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //Auto attack only works for main weapons
        if(Input.GetMouseButtonDown(1))
        {
            autoAttack = !autoAttack;
        }

        //Prevent player from attacking again until cooldown is over
        if (Input.GetMouseButton(0) || autoAttack)
        {
            //Pistol attack for soldier
            if(companionSystem.playerCharacter == "soldier")
            {
                shoot.SetActive(true);
                fire.SetActive(false);
                swing.SetActive(false);
                companionSystem.characterMS = 7f;
            }
            //Flamethrower attack for mercenary
            else if (companionSystem.playerCharacter == "mercenary")
            {
                fire.SetActive(true);
                shoot.SetActive(false);
                swing.SetActive(false);
                //Movement decrease when using flamethrower
                companionSystem.characterMS = 3f;
            }
            //Sword attack for swordsman
            else
            {
                swing.SetActive(true);
                fire.SetActive(false);
                shoot.SetActive(false);
                companionSystem.characterMS = 7f;
            }
        }
        else
        {
            fire.SetActive(false);
            swing.SetActive(false);
            shoot.SetActive(false);
            companionSystem.characterMS = 7f;
        }
    }
}
