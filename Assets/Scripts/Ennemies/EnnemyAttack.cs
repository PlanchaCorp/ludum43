using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAttack : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
       if (collision.CompareTag("Player"))
        {
            Debug.Log(collision.tag);
            gameObject.GetComponentInParent<Animator>().SetBool("IsAttacking", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<Animator>().SetBool("IsAttacking", false);
        }
    }
}
