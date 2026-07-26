using UnityEngine;

public class Pistol : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer sr;
    [SerializeField] private CharacterStats cs;
    [SerializeField] private Sprite pistol;

    private void Awake()
    {
        mainCamera = Camera.main;
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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

        //Current aim
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Flip sprite to display correctly
        if(angle > 90 && angle <= 180 || angle >= -180 && angle < -90)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }

        ////Render weapon below character sprite
        //if (angle > 45 && angle < 135)
        //{
        //    sr.sortingLayerName = "Temporary";
        //}
        //else
        //{
        //    sr.sortingLayerName = "Weapon";
        //}
    }
}
