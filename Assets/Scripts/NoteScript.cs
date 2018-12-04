using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteScript : MonoBehaviour
{

    private string message;
    private ObjectiveManager objective;
    // Use this for initialization
    void Start()
    {
        objective = GameObject.FindGameObjectWithTag("Objectives").GetComponent<ObjectiveManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DisplayNote();
        }
    }

    private void DisplayNote()
    {
        if(message == null)
        {
            message = objective.Message;
        }
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
        Debug.Log(message);
        objective.DisplayMessage(message);
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        objective.HideMessage();
    }
    private void OnMouseDown()
    {
        objective.HideMessage();
    }
}
