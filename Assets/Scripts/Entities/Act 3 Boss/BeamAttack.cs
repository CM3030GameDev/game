using UnityEngine;

public class BeamAttack : MonoBehaviour
{
    [SerializeField] private LayerMask pillarLayer;
    [SerializeField] private Character character;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 100f, pillarLayer.value);

        //Beam hits pillar
        if (hit)
        {
            //Beam stops before pillar
            transform.localScale = new Vector3(hit.distance, 2f, 1f);
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
            character.CharacterAttacked(50);
            character.GrantInvulnerability(2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            character.CharacterAttacked(50);
            character.GrantInvulnerability(2f);
        }
    }
}
