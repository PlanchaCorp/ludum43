using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{


    
    private Transform player;

    private EnnemyScript ennemy;

    private Animator stateMachine;

    private bool IsEnrage;


    LayerMask mask;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        IsEnrage = false;
        stateMachine = gameObject.GetComponentInParent<Animator>();
        ennemy = gameObject.GetComponentInParent<EnnemyScript>();
        mask = LayerMask.GetMask("Player");
        mask |= LayerMask.GetMask("Obstacle");

    }
    private void Update()
    {

        Investigate();


    }

    void Investigate()
    {
        Vector3 targetPos = player.position - gameObject.transform.position;

        Debug.DrawRay(gameObject.transform.position, targetPos, Color.red);
        Debug.DrawRay(gameObject.transform.position, Vector2.right * Vector2.up, Color.green);
        Debug.DrawRay(gameObject.transform.position, Vector2.right * Vector2.down, Color.green);
        RaycastHit2D hitInfo = Physics2D.Raycast(gameObject.transform.position, targetPos, ennemy.viewDistance, mask);


        if (hitInfo.collider != null)
        {

            if (((hitInfo.collider.CompareTag("Player") && Vector2.Angle(gameObject.transform.right, targetPos) < ennemy.viewAngle) || IsEnrage) && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInvisible())
            {
                Debug.DrawRay(gameObject.transform.position, targetPos, Color.blue);
                TriggerPatrol(false);
                TriggerTrack(false);
                TriggerWatching(false);
                TriggerChase(true);
            }
            else
            {
                Debug.DrawRay(gameObject.transform.position, targetPos, Color.red);
                TriggerChase(false);
                TriggerTrack(true);
            }

        }

    }

    private void TriggerWatching(bool trigger)
    {
        stateMachine.SetBool("IsWatching", trigger);
    }

    private void TriggerTrack(bool trigger)
    {
        stateMachine.SetBool("IsTracking", trigger);
    }

    private void TriggerFear(bool trigger)
    {
        stateMachine.SetBool("IsFearing", trigger);
    }

    public void Enrage(Transform target)
    {
        Vector3 targetPos = target.position - gameObject.transform.position;
        RaycastHit2D hitInfo = Physics2D.Raycast(gameObject.transform.position, targetPos, ennemy.viewDistance);
        IsEnrage = false;
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Ennemy") && Vector3.Angle(gameObject.transform.right, targetPos) < ennemy.viewAngle && !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInvisible())
            {
                Debug.DrawLine(gameObject.transform.position, target.position, Color.magenta);

                IsEnrage = true;
            }
        }

    }
    void TriggerChase(bool chase)
    {
        stateMachine.SetBool("IsChasing", chase);
    }

    void TriggerPatrol(bool patrol)
    {
        stateMachine.SetBool("IsPatrolling", patrol);
    }

    public void LoseTargetTrack()
    {
        StartCoroutine(LoseTrack());
    }
    IEnumerator LoseTrack()
    {
        yield return new WaitForSeconds(1f);
        if (stateMachine.GetBool("IsTracking"))
        {
            stateMachine.SetBool("IsWatching", true);
            stateMachine.SetBool("IsTracking", false);
        }
    
    }

    public void GiveUp()
    {
        StartCoroutine(ResetPosition());
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(5f);
        if (stateMachine.GetBool("IsWatching"))
        {
            stateMachine.SetBool("IsWatching", false);
            StartCoroutine(ennemy.TeleportToSpawn());
        }
    }

    public IEnumerator Scare(float fearTime)
    {
        if (!stateMachine.GetBool("IsFearing"))
        {
            TriggerFear(true);
            TriggerChase(false);
            TriggerTrack(false);
            TriggerPatrol(false);
            TriggerWatching(false);
        }
        yield return new WaitForSeconds(fearTime);
        TriggerFear(false);
        TriggerWatching(true);
    }
}
