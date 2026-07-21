using UnityEngine;

public class WeaponDirection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private CompanionSystem companionSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = weaponSystem.equippedDict[weaponSystem.currentWeapon];

        //Get mouse position in screen pixels
        Vector3 screenPosition = Input.mousePosition;

        //Convert screen pixels to 3D world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        //Convert 3D world coordinate to 2D 
        worldPosition.z = 0f;

        //Direction vector from player to mouse
        Vector2 direction = worldPosition - transform.position;

        //Convert angle from radian to degree
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Convert negative angle to positive angle
        if(angle < 0)
        {
            angle += 360;
        }

        //Render weapon below character sprite
        if(angle > 45 && angle < 135)
        {
            spriteRenderer.sortingLayerName = "Temporary";
        }
        else
        {
            spriteRenderer.sortingLayerName = "Weapon";
        }

        //Correctly display weapon sprite when facing left
        if (angle > 135 && angle < 225)
        {
            transform.rotation = Quaternion.Euler(180, 0, 360 - angle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
