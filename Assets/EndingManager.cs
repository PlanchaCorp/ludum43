using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {

    public bool IsBadEnding;

    public GameObject GoodEnding;
    public GameObject BadEnding;
    // Use this for initialization
    void Start () {
        if (IsBadEnding)
        {
            GoodEnding.GetComponent<Image>().enabled = false;
        } else
        {
            BadEnding.GetComponent<Image>().enabled = false;
        }
	}
	

}
