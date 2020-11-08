using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;

    public InputField usernameField;

    public GameObject chatBoxOBJ;

    public InputField chatBox;

    public TextMeshProUGUI recentUser;

    public TextMeshProUGUI recentMessage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Insance already exists, destroying object");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();

        chatBoxOBJ.SetActive(true);

    }

    public void SendMessage()
    {
        if (chatBox.text.Trim(' ') != "" )
        {
            ClientSend.chatMessageReceived();

            chatBox.Select();
            chatBox.text = "";
        }
    }


}
