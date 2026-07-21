using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fireSprite;
    [SerializeField] private BoxCollider2D hitBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Dynamically change hitbox according to each sprite in animation
        hitBox.size = fireSprite.sprite.bounds.size;
        //Offset by half the sprite size since each sprite pivot is left
        hitBox.offset = new Vector2(hitBox.size.x / 2, 0);
    }
}
