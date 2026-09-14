using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private FinalBossOne finalBossOne;

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
        //End smokescreen animation after 3 cycles
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 3f)
        {
            //Stop smoke sound effect
            PhaseOneManager.Instance.StopAudio();
            //End boss attack animation
            bossAnimator.SetBool("attack", false);
            //Disable smokescreen animation
            gameObject.SetActive(false);
            //Update boss attack state
            finalBossOne.attacking = false;
            //Refresh next attack timer
            finalBossOne.attackTime = 0f;
            //Refresh next random attack
            finalBossOne.randomNum = Random.Range(0f, 1f);
        }
    }
}
