using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    /// <summary>
    /// Stun duration in seconds
    /// </summary>
    private float stunDuration = 2.0f;

    /// <summary>
    /// Destroying object when colliding with something (except the player)
    /// </summary>
    /// <param name="collision">Collision data</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ennemy")
        {
            collision.collider.gameObject.GetComponent<EnnemyScript>().Stun(stunDuration);
            Destroy(gameObject);
        } else if (collision.collider.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Set the new stun duration
    /// </summary>
    /// <param name="duration">New stun duration</param>
    public void SetStunDuration(float duration)
    {
        stunDuration = duration;
    }
}
