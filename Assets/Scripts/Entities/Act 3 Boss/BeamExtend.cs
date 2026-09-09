using UnityEngine;

public class BeamExtend : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    //Layer for pillar and safe zone
    [SerializeField] private LayerMask layers;
    [SerializeField] private Sprite box;
    [SerializeField] private Sprite beam;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        //Hitbox sprite
        sr.sprite = box;
        //Disable box collider when showing hitbox
        boxCollider.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast beam body
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 100f, layers.value);

        //Beam body hits pillar or safezone
        if (hit)
        {
            //Beam stops before pillar or safezone
            transform.localScale = new Vector3(hit.distance, 2f, 1f);
        }
        //Beam body is not hitting pillar or safezone
        else
        {
            //Beam extends
            transform.localScale = new Vector3(100f, 2f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(30, 0.5f);
        }
    }
}
