using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public Vector2 dashNormalized;
    private Vector2 targetPos;
    public int dashSpeed;
    [SerializeField] private GameObject character;
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private LayerMask layers;

    private void OnEnable()
    {
        if(character.transform.position.x > transform.position.x)
        {
            finalBoss.sr.flipX = false;
        }
        else
        {
            finalBoss.sr.flipX = true;
        }

        Vector2 dashDirection = character.transform.position - transform.position;
        dashNormalized = dashDirection.normalized;

        //Raycast to check if there is obstacle between boss and player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashNormalized, 15f, layers.value);

        //Dash to either obstacle position or player position
        if (hit)
        {
            targetPos = hit.point;
        }

        //Boss is currently dashing
        finalBoss.dashing = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Stop dashing after reaching target position
        if (Vector2.Distance(transform.position, targetPos) < 2f)
        {
            finalBoss.dashing = false;
            finalBoss.animator.SetBool("dash", false);
            finalBoss.randomNum = Random.Range(0f, 1f);
            finalBoss.attackTime = 0f;
            finalBoss.attacking = false;
            gameObject.SetActive(false);
        }
    }
}
