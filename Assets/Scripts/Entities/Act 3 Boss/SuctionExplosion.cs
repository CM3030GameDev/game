using UnityEngine;

public class SuctionExplosion : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject suction;
    [SerializeField] private FinalBoss finalBoss;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //Enable collider component on suction
        suction.GetComponent<CircleCollider2D>().enabled = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            //Disable gameobject after animation ends
            gameObject.SetActive(false);
            //Disable collider component on suction
            suction.GetComponent<CircleCollider2D>().enabled = false;
            //Switch attacking state to false
            finalBoss.attacking = false;
        }
    }
}
