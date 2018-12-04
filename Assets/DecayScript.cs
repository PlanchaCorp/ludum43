using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Decay());
	}
	


    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
