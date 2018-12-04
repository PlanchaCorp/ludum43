using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    /// <summary>
    /// Power of the fireball thrown
    /// </summary>
    private float FIREBALLPOWER = 100f;
    /// <summary>
    /// Duration of the stun when an ennemy is hit
    /// </summary>
    private float STUNDURATION = 2.0f;
    /// <summary>
    /// Mana cost
    /// </summary>
    private float BLOODCOST = 8;

    /// <summary>
    /// Used to remember the fireball origin (aka the player)
    /// </summary>
    private Vector2 origin;
    /// <summary>
    /// Used to remember the fireball target position (aka the mouse position when the click happens)
    /// </summary>
    private Vector2 targetPosition;
    /// <summary>
    /// Direction calculed from the origin to the fireball target position
    /// </summary>
    private Vector2 direction;
    /// <summary>
    /// Future fireball object that will be created
    /// </summary>
    private GameObject fireball;

    /// <summary>
    /// Stun in action
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
        // Calculating positions and vector for future fireball
        origin = gameObject.transform.position;
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = targetPosition - origin;
        // Instantiating the fireball and applying stuff
        fireball = new GameObject("Fireball");
        fireball.AddComponent<SpriteRenderer>();
        fireball.AddComponent<FireballBehaviour>().SetStunDuration(STUNDURATION);
        fireball.AddComponent<Rigidbody2D>().gravityScale = 0;
        fireball.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Fireball");
        CircleCollider2D circleCollider2D = fireball.AddComponent<CircleCollider2D>();
        Vector2 playerSafeDistance = direction.normalized * 1.1f * GetComponent<CapsuleCollider2D>().bounds.size.y;
        fireball.transform.position = new Vector2(transform.position.x, transform.position.y) + playerSafeDistance;
        // Triggering the safe delete
        StartCoroutine(Disappear(10));
    }

    /// <summary>
    /// Increasing fireball force while alive
    /// </summary>
    void Update ()
    {
        if (fireball != null)
        {
            fireball.GetComponent<Rigidbody2D>().AddForce(direction * FIREBALLPOWER * Time.deltaTime);
        }
    }

    /// <summary>
    /// Disappear when alive for too long
    /// </summary>
    /// <param name="timeBeforeDisappearance">Time before the fireball disappears</param>
    /// <returns>Yielding some time</returns>
    IEnumerator Disappear(float timeBeforeDisappearance)
    {
        yield return new WaitForSeconds(timeBeforeDisappearance);
        if (fireball != null)
        {
            Destroy(fireball);
        }
        Destroy(this);
    }
}
