using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] private SpriteRenderer swingSprite;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Dynamically change hitbox according to each sprite in animation
        hitBox.size = swingSprite.sprite.bounds.size;
        //Offset by half the sprite size since each sprite pivot is left
        hitBox.offset = new Vector2(hitBox.size.x / 2, 0);

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
        if (angle < 0)
        {
            angle += 360;
        }

        //Render weapon below character sprite
        if (angle > 45 && angle < 135)
        {
            swingSprite.sortingLayerName = "Temporary";
        }
        else
        {
            swingSprite.sortingLayerName = "Weapon";
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
