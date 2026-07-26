using UnityEngine;

public class Swing : MonoBehaviour
{
    private Camera mainCamera;
    private BoxCollider2D box;
    [SerializeField] private SpriteRenderer swingSprite;

    private void Awake()
    {
        mainCamera = Camera.main;
        box = GetComponent<BoxCollider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Dynamically change hitbox according to each sprite in animation
        box.size = swingSprite.sprite.bounds.size;
        //Offset by half the sprite size since each sprite pivot is left
        box.offset = new Vector2(box.size.x / 2, 0);

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

        //Correctly display weapon sprite
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
            Quaternion.AngleAxis(180, Vector3.up);
            Quaternion.AngleAxis(180, Vector3.right);
            Quaternion.AngleAxis(360 - angle, Vector3.forward);
            //transform.rotation = Quaternion.Euler(180, 0, 360 - angle);
            transform.eulerAngles = new Vector3(180, 0, 360 - angle);
        }
        else
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.Euler(0, 0, angle);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
