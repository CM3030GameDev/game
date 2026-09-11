using UnityEngine;

public class BeamCharge : MonoBehaviour
{
    //Beam charge box collider
    private BoxCollider2D chargeCollider;
    private Animator animator;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private SpriteRenderer beam;
    [SerializeField] private Sprite beamSprite;
    //Beam body box collider
    [SerializeField] private BoxCollider2D bodyCollider;
    [SerializeField] private BeamRotate beamRotate;
    [SerializeField] private GameObject beams;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        chargeCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Beam charge starting animation finished
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Beam_Charge_Start") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Change beam body sprite from hitbox to beam
            beam.sprite = beamSprite;
            //Enable box collider on beam charge
            chargeCollider.enabled = true;
            //Enable box collider on beam body
            bodyCollider.enabled = true;
            //Beam can start to rotate
            beamRotate.isRotating = true;
        }

        //Beam charge ending animation finished
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Beam_Charge_End") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Boss attack ending animation
            bossAnimator.SetBool("attack", false);
            //Disable beams
            beams.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(30);
            character.GrantInvulnerability(0.5f);
        }
    }
}
