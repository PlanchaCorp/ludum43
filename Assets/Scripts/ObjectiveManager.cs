using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectiveManager : MonoBehaviour {

    public MessageData messageData;

    private Queue<string> _messageQueue;

    private Canvas noteCanvas;

    private void Start()
    {
        if (messageData != null)
        {
            _messageQueue = new Queue<string>(messageData.Dialog);
        }
        noteCanvas = transform.Find("NoteCanvas").GetComponent<Canvas>();

    }

    public string Message
    {
        get { return _messageQueue.Dequeue(); }
    }

    public void DisplayMessage(string message)
    {
        TextMeshProUGUI note = gameObject.transform.Find("NoteCanvas/Panel/NoteText").GetComponent<TextMeshProUGUI>();
        Debug.Log(message);
        note.text = message;
        noteCanvas.enabled = true;
    }
    public void HideMessage()
    {
        noteCanvas.enabled = false;
    }
}
