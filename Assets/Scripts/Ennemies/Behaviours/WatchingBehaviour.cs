using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchingBehaviour : StateMachineBehaviour {

    bool clockWise;
    float baseAngle;
    EnnemyScript ennemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        baseAngle = animator.transform.Find("Vision").transform.rotation.z;
        clockWise = Random.value > 0.5f;
        ennemy = animator.gameObject.GetComponent<EnnemyScript>();
        ennemy.SetTarget(null);
        ennemy.CanMove(false);

        ennemy.Interogation();
        animator.transform.Find("Vision").GetComponent<Sight>().GiveUp();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        Vector3 angleRotation = Vector3.forward * 40 * Time.deltaTime;
        if (clockWise)
        {
            animator.transform.Find("Vision").Rotate(angleRotation);
        } else
        {
            animator.transform.Find("Vision").Rotate(-angleRotation);
        }
        float actualAngle = animator.transform.Find("Vision").transform.rotation.z;
        if(Mathf.Abs( baseAngle - actualAngle) > 0.3f)
        {
            clockWise = !clockWise;
        }
              

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
