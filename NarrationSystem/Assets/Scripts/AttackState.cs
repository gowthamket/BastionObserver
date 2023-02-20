using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex == 1) {
            animator.SetLayerWeight(1, 1);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.IsInTransition(layerIndex))
        {
            return;
        }

        if (animator.GetBool("isAttacking"))
        {
            return;
        }

        CheckSwitchStates(animator, layerIndex);
    }

    void CheckSwitchStates(Animator animator, int layerIndex)
    {
        if (animator.GetBool("isFalling"))
        {
            animator.CrossFadeInFixedTime("Falling", .1f);
        }
        else if (animator.GetBool("isGrounded"))
        {
            bool isRunning = animator.GetBool("isRunning");
            bool isWalking = animator.GetBool("isWalking");

            if (!isRunning && isWalking)
            {
                animator.CrossFadeInFixedTime("Walk", .1f, layerIndex);
            }
            else if (isRunning && isWalking)
            {
                animator.CrossFadeInFixedTime("Run", .1f, layerIndex);
            }
            else
            {
                animator.CrossFadeInFixedTime("Idle", .1f, layerIndex);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex == 1)
        {
            animator.SetLayerWeight(1, 0);
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
