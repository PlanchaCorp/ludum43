using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu]
public class MessageData : ScriptableObject
{
    public string levelName;

    public int sizeX;
    public int SizeY;

    public int minRoomQuantity;

    public int maxRoomQuantity;

    [TextArea]
    public List<string> Dialog;
   
}