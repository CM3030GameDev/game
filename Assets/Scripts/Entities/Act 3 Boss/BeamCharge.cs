using UnityEngine;

public class BeamCharge : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private SpriteRenderer beam;
    [SerializeField] private Sprite beamSprite;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BeamRotate beamRotate;
    [SerializeField] private GameObject beams;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //After beam charge animation finishes
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Beam_Charge") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Change beam body sprite from hitbox to beam
            beam.sprite = beamSprite;
            //Enable box collider on beam
            boxCollider.enabled = true;
            //Beam can start to rotate
            beamRotate.isRotating = true;
        }

        //After beam end animation finishes
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Beam_Charge_End") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            beams.SetActive(false);
        }
    }
}
