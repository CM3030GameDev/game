using UnityEngine;

public class BeamExtend : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask buildingsLayer;
    [SerializeField] private Sprite box;
    [SerializeField] private Sprite beam;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 100f, buildingsLayer.value);

        //Beam hits pillar
        if (hit)
        {
            if(hit.distance > 5f)
            {
                //Beam stops before pillar
                transform.localScale = new Vector3(hit.distance + 5f, 2f, 1f);
            }
            else
            {
                //Beam stops before pillar
                transform.localScale = new Vector3(hit.distance, 2f, 1f);
            }
        }
        //Beam is not hitting pillar
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
            character.CharacterAttacked(30);
            character.GrantInvulnerability(0.5f);
        }
    }
}
