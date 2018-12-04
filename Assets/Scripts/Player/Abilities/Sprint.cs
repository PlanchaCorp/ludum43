using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    /// <summary>
    /// Speed multiplier
    /// </summary>
    private float SPRINTAMOUNT = 2.0f;
    /// <summary>
    /// Duration of the sprint
    /// </summary>
    private float SPRINTDURATION = 5.0f;
    /// <summary>
    /// Mana cost
    /// </summary>
    private float BLOODCOST = 5;

    /// <summary>
    /// Sprinting !
    /// </summary>
    void Start ()
    {
        // Checking that player has enough blood, and pay the price
        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        if (!playerController.CanCast(BLOODCOST))
        {
            Destroy(this);
            return;
        }
        playerController.ReduceBlood(BLOODCOST);
        // Accelerating
        GetComponent<PlayerController>().Accelerate(SPRINTAMOUNT, SPRINTDURATION);
        Destroy(this);
	}
}
