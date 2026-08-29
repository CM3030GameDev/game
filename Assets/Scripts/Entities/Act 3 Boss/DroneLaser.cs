using UnityEngine;

public class DroneLaser : StateMachineBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Shoot laser
        GameObject laser = Instantiate(laserPrefab, animator.transform.position + (animator.transform.right * 2.5f), Quaternion.identity);
        if(laser != null)
        {
            Rigidbody2D rb = laser.GetComponent<Rigidbody2D>();
            rb.linearVelocity = animator.transform.right * laserSpeed;
        }

        animator.GetComponent<Drone>().attacking = false;
        animator.GetComponent<Drone>().attackTime = 0f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
