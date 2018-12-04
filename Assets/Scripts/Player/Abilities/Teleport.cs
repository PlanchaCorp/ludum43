using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Teleport : MonoBehaviour {
    /// <summary>
    /// Fixed amount of power to teleport forward
    /// </summary>
    private float POWER = 2000;
    /// <summary>
    /// Fixed amount of repulsion when colliding while TPing
    /// </summary>
    private float COLLISIONREPULSION = 10;
    /// <summary>
    /// Mana cost
    /// </summary>
    private float BLOODCOST = 18;

    /// <summary>
    /// Remembering player's color
    /// </summary>
    private Color playerColor;
    /// <summary>
    /// Remembering player position before teleportation
    /// </summary>
    private Vector3 playerOldPosition;
    private int wallCollision = 0;
    
    /// <summary>
    /// Component initialization
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
        // Handle teleportation
        InitializeTeleportation();
        StartCoroutine(ExecuteTeleportation());
    }

    /// <summary>
    /// Start the teleportation and change player state
    /// </summary>
    void InitializeTeleportation()
    {
        GetComponent<Collider2D>().enabled = false;
        playerColor = GetComponent<SpriteRenderer>().color;
        playerOldPosition = transform.position;
        GetComponent<SpriteRenderer>().color = Color.grey;
    }

    /// <summary>
    /// Coroutine to teleport the player and wait some time
    /// </summary>
    /// <returns>Yielding some time</returns>
    IEnumerator ExecuteTeleportation()
    {
        // First teleportation
        Vector2 playerToMouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
        GetComponent<Rigidbody2D>().AddForce(playerToMouseVector.normalized * POWER);
        yield return new WaitForSeconds(0.3f);
        // Ending teleportation
        GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        if (wallCollision > 0)
        {
            StartCoroutine(RollbackTeleportation());
        } else
        {
            FinishTeleportation();
        }
    }

    /// <summary>
    /// Rollback the teleportation when collisionned
    /// </summary>
    /// <returns>Yielding some time</returns>
    IEnumerator RollbackTeleportation()
    {
        Vector2 playerToRollbackVector = playerOldPosition - gameObject.transform.position;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(playerToRollbackVector.normalized * POWER);
        yield return new WaitForSeconds(0.3f);
        FinishTeleportation();
    }

    /// <summary>
    /// End the teleportation and restore player state
    /// </summary>
    void FinishTeleportation()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<Collider2D>().enabled = true;
        Destroy(this);
    }

    /// <summary>
    /// Counting collision count when solid
    /// </summary>
    /// <param name="collision">Collision data</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Obstacle")
        {
            wallCollision++;
            Vector3 repulsionDirection = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
            repulsionDirection = -repulsionDirection.normalized;
            GetComponent<Rigidbody2D>().AddForce(repulsionDirection * COLLISIONREPULSION);
        }
    }

    /// <summary>
    /// Counting collision count when solid
    /// </summary>
    /// <param name="collision">Collision data</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Obstacle")
        {
            wallCollision--;
        }
    }
}
