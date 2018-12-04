using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{

    Queue<GameObject> jalons;
    EnnemyScript ennemy;
    Transform nextPosition;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ennemy = animator.gameObject.GetComponent<EnnemyScript>();
        ennemy.CanMove(true);
        if (ennemy.jalons != null)
        {
            jalons = new Queue<GameObject>(ennemy.jalons);
        }
        GameObject jalon = jalons.Dequeue();
        if (jalon)
        {
            nextPosition = jalon.transform;
        } else
        {
            nextPosition = ennemy.transform;
        }
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ennemy.canPatrol.Equals(true))
        {
            walkToObjective(nextPosition);
            float angle = Vector2.SignedAngle(animator.transform.Find("Vision").right, (nextPosition.position - animator.transform.position));

            if (jalons.Count == 0)
            {
                // animator.SetBool("IsPatrolling", false);
                jalons = new Queue<GameObject>(ennemy.jalons);
            }
            animator.transform.Find("Vision").Rotate(new Vector3(0, 0, angle));
            if (Vector3.Distance(animator.transform.position,nextPosition.position)<0.2f)
            {
                GameObject jalon = jalons.Dequeue();
                if (jalon != null)
                {
                    nextPosition = jalon.transform;
                } else
                {
                    nextPosition = ennemy.transform;
                }
            }
        }
       

    }

    private void walkToObjective(Transform transform)
    {
        ennemy.SetTarget(transform);
     
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
