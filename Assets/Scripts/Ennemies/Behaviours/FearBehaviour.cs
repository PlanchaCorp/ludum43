using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearBehaviour : StateMachineBehaviour {

    Vector3 lastPlayerPos;
    Vector3 fearTranslation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        lastPlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        fearTranslation = (animator.transform.position - lastPlayerPos).normalized * animator.gameObject.GetComponent<EnnemyScript>().speed * Time.deltaTime;
        animator.transform.GetComponent<EnnemyScript>().Fear();
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.transform.position = animator.transform.position + fearTranslation;

        float angle = Vector2.SignedAngle(animator.transform.Find("Vision").right, -(lastPlayerPos - animator.transform.position));

        animator.transform.Find("Vision").Rotate(new Vector3(0, 0, angle));
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
