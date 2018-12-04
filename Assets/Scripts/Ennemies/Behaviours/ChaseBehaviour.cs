using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ChaseBehaviour : StateMachineBehaviour
{


    private Transform player;

    private List<GameObject> ennemies;

    CameraShakeInstance instance;


    //  OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animator.gameObject.name + "Start chasing");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ennemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Ennemy"));

        animator.gameObject.GetComponent<EnnemyScript>().CanMove(true);
        animator.gameObject.GetComponent<EnnemyScript>().SetTarget(player);
        animator.gameObject.GetComponent<EnnemyScript>().Exclamation();
        instance = CameraShaker.Instance.StartShake(4f, 0.5f, 15f);
        
       

    }

    //  OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(GameObject ennemy in ennemies)
        {
            ennemy.GetComponentInChildren<Sight>().Enrage(animator.transform);
        }

        

        //animator.transform.position = Vector2.MoveTowards(animator.transform.position, player.position, animator.gameObject.GetComponent<EnnemyScript>().speed * Time.deltaTime);
        float angle =  Vector2.SignedAngle(animator.transform.Find("Vision").right, (player.position - animator.transform.position));

         animator.transform.Find("Vision").Rotate(new Vector3(0,0,angle));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        instance.StartFadeOut(1);
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
