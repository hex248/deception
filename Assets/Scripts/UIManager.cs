using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;

    public InputField usernameField;

    public static bool loggedIn;

    public GameObject chatBoxOBJ;

    public static int maxMessages = 25;


    public static GameObject chatPanel, textObject;

    public GameObject chat_panel, text_prefab;

    public static InputField chat_input;

    public InputField chatInput;

    [SerializeField]
    static List<Message> messageList = new List<Message>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }

        chatPanel = chat_panel;
        textObject = text_prefab;
        chat_input = chatInput;
    }

    void Update()
    {
        if (!loggedIn)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ConnectToServer();
            }
        }
        else if (loggedIn)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessage();

                chat_input.ActivateInputField();
            }
        }
    }

    public void ConnectToServer()
    {
        if (usernameField.text.Trim(' ').Length <= 20)
        {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            usernameField.text = usernameField.text.Trim(' ');
            Client.instance.ConnectToServer();

            chatBoxOBJ.SetActive(true);
            loggedIn = true;
        }
    }

    public void SendMessage()
    {
        if (chatInput.text.Trim(' ') != "" && chatInput.text.Length <= 100)
        {
            chatInput.text = chatInput.text.Trim(' ');

            ClientSend.chatMessageReceived();

            chatInput.text = "";
        }
    }

    public static void SendMessageToChat(string username, string content)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }
        Message newMessage = new Message(username, content);

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = $"{newMessage.username}: {newMessage.content}";

        messageList.Add(newMessage);

        chat_input.ActivateInputField();
    }


}
