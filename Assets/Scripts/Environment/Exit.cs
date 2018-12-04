using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController == null)
        {
            GameObject managerGameObject = Instantiate(new GameObject());
            managerGameObject.tag = "GameController";
            Manager manager = managerGameObject.AddComponent<Manager>();
            DontDestroyOnLoad(manager);
        }
        if (collider.tag == "Player")
        {
            Manager manager = gameController.GetComponent<Manager>();
            manager.SkipLevel();
            manager.GoToSkillSelection();
        }
    }
}
