using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMain : MonoBehaviour {

    // Use this for initialization
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject CreditMenu;
    
    void Start () {
		
	}
	
public void Play(int prmPlay)
    {
        SceneManager.LoadScene(prmPlay);
    }
public void Credit()
    {
        MainMenu.SetActive(false);
        CreditMenu.SetActive(true);

    }
    public void Back()
    {
        MainMenu.SetActive(true);
        CreditMenu.SetActive(false);

    }
    public void Quit()
    {
        Application.Quit();
    }
}
