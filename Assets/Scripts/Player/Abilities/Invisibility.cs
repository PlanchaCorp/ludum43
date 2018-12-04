using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour
{
    /// <summary>
    /// Time available when invisible
    /// </summary>
    private float INVISIBILITYDURATION = 5;
    /// <summary>
    /// Mana cost
    /// </summary>
    private float BLOODCOST = 15;

    /// <summary>
    /// Disappearing in the shadows !
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
        // Turning invisible
        GetComponent<SpriteRenderer>().color = Color.cyan;
        GetComponent<PlayerController>().Disappear(INVISIBILITYDURATION);
        Destroy(this);
    }
}
