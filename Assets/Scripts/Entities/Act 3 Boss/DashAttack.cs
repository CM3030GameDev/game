using UnityEngine;

public class DashAttack : MonoBehaviour
{
    private Vector2 characterPos;
    public Vector2 dashDirection;
    public int dashSpeed;
    [SerializeField] private GameObject character;
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private LayerMask buildingsLayer;

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

        //Target player current position
        characterPos = character.transform.position;
        //Direction towards player current position
        dashDirection = (character.transform.position - transform.position).normalized;

        //Raycast to check if there is obstacle between boss and player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dashDirection, 15f, buildingsLayer.value);

        //Collide with obstacle
        if (hit)
        {
            //Dash till obstacle
            characterPos = hit.point - (dashDirection * 5f);
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
        if (Vector2.Distance(transform.position, characterPos) < 1f)
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
