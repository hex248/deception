using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Message
{
    public string username;

    public string content;

    public TextMeshProUGUI textObject;

    public Message(string _username, string _content)
    {
        this.username = _username;
        this.content = _content;
    }
}
