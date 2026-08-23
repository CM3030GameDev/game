using UnityEngine;

public class MobDeath : StateMachineBehaviour
{
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private GameObject expOrbPrefab;
    [SerializeField] private int expReward = 10;

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
        animator.gameObject.SetActive(false);
        enemySystem.enemyLeft -= 1;

        // Spawn exp orb on death
        if (expOrbPrefab != null)
        {
            GameObject orb = Instantiate(expOrbPrefab, animator.transform.position, Quaternion.identity);
            orb.GetComponent<ExpOrb>().SetExp(expReward);
        }
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
