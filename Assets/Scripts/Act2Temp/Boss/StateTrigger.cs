using UnityEngine;
using System;

public class StateTrigger : StateMachineBehaviour
{
    public Action OnStateExitAction;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OnStateExitAction?.Invoke();
    }
}
