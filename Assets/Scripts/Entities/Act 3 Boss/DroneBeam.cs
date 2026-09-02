using UnityEngine;

public class DroneBeam : StateMachineBehaviour
{
    private float timer;
    private GameObject beam;
    private Drone drone;
    [SerializeField] private GameObject beamPrefab;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        drone = animator.GetComponent<Drone>();

        //Instantiate beam as a child gameobject of drone
        beam = Instantiate(beamPrefab, animator.transform);

        //Adjust beam's local position relative to the parent drone position
        beam.transform.localPosition += animator.transform.right;
        beam.transform.localPosition += animator.transform.up * 0.017f;
        //Start timer
        timer = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Destroy beam after 5 seconds
        if(timer >= 5f) 
        {
            drone.attacking = false;
            drone.attackTime = 0f;
            drone.beaming = false;
            Destroy(beam);
            animator.SetTrigger("beam_end");
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
