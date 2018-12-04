using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMain : MonoBehaviour
{

    Manager manager;

    // Use this for initialization
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject CreditMenu;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Manager>();

    }

    public void Play()
    {
        // SceneManager.LoadScene(prmPlay);
        manager.GoToTutoLevel();
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
