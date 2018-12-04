using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TrackBehaviour : StateMachineBehaviour
{

    Transform lastPlayerPos;
    float angle;
   

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lastPlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
        animator.gameObject.GetComponent<EnnemyScript>().SetTarget(lastPlayerPos);
        angle = Vector2.SignedAngle(animator.transform.Find("Vision").right, (lastPlayerPos.position - animator.transform.position));
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.position = Vector2.MoveTowards(animator.transform.position, lastPlayerPos, animator.gameObject.GetComponent<EnnemyScript>().speed * Time.deltaTime);

        UnityEngine.Random.Range(-10, 10);

        animator.transform.Find("Vision").Rotate(new Vector3(0, 0, angle * UnityEngine.Random.Range(-10, 10)));

        animator.transform.Find("Vision").GetComponent<Sight>().LoseTargetTrack();
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
