using UnityEngine;

public class PhaseOneDash : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private FinalBossOne finalBossOne;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Stop dashing when close to player current position
        if (Vector2.Distance(transform.position, PhaseOneManager.Instance.characterTransform.position) < 5f)
        {
            finalBossOne.dashing = false;
            gameObject.SetActive(false);
            //Revert boss speed to original speed
            finalBossOne.currentSpeed = characterStats.moveSpeed - 2;
            //End boss dashing animation
            animator.SetBool("dash", false);
            //Update boss attack state
            finalBossOne.attacking = false;
            //Refresh next attack timer
            finalBossOne.attackTime = 0f;
            //Refresh next random attack
            finalBossOne.randomNum = Random.Range(0f, 1f);
        }
    }
}
