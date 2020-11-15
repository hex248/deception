using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Net;
using System.Linq;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;

    public InputField usernameField;

    public static bool loggedIn;

    public static GameObject lobbySelectionStaticOBJ;
    public GameObject lobbySelectionOBJ;
    
    public static GameObject chatBoxStaticOBJ;
    public GameObject chatBoxOBJ;

    public static int maxMessages = 25;


    public static GameObject chatPanel, textObject;

    public GameObject chat_panel, text_prefab;

    public static InputField chat_input;

    public InputField chatInput;

    [SerializeField]
    static List<Message> messageList = new List<Message>();

    static List<Lobby> currentLobbyList = new List<Lobby>();

    static List<GameObject> lobbyListObj = new List<GameObject>();

    public GameObject lobbyContent;

    public GameObject lobbyEntryPrefab;



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

        lobbySelectionStaticOBJ = lobbySelectionOBJ;
        chatBoxStaticOBJ = chatBoxOBJ;

        currentLobbyList = Client.lobbyList;
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

        if (!lobbyListsEqual(currentLobbyList, Client.lobbyList))
        {
            currentLobbyList = Client.lobbyList;
        }
    }

    public void ConnectToServer()
    {
        if (usernameField.text.Trim(' ').Length <= 20)
        {
            startMenu.SetActive(false);
            lobbySelectionOBJ.SetActive(true);
            usernameField.interactable = false;
            usernameField.text = usernameField.text.Trim(' ');
            Client.instance.ConnectToServer();
            loggedIn = true;
            RefreshLobbies();
        }
    }

    public void createLobby()
    {
        ClientSend.createLobbyRequest(Client.player);
    }

    public void joinLobbyRequest(string lobbyId)
    {
        if (Client.player.currentLobby != lobbyId)
        {
            Client.player.currentLobby = null;
            ClientSend.joinLobbyRequest(lobbyId);
        }
    }

    public void RefreshLobbies()
    {
        currentLobbyList = Client.lobbyList;
        // Destroys all previously existing lobby entries
        try
        {
            foreach (GameObject obj in lobbyListObj)
            {
                Destroy(obj.gameObject);
                lobbyListObj.Remove(obj);
            }
        }
        catch
        {
            return;
        }

        foreach (Lobby lobby in currentLobbyList)
        {
            //Debug.Log($"Lobby Owner: {lobby.ownerName}, Lobby Code: {lobby.id}");

            GameObject newLobby = Instantiate(lobbyEntryPrefab, lobbyContent.transform);

            var script = newLobby.GetComponent<Lobby>();

            script.id = lobby.id;
            script.ownerIP = lobby.ownerIP;
            script.ownerName = lobby.ownerName;
            script.players = lobby.players;
            script.playerCount = lobby.playerCount;

            script.idObj.text = script.id;
            script.usernameObj.text = script.ownerName;
            script.playerCountObj.text = $"{script.playerCount}/{script.maxPlayers}";

            lobbyListObj.Add(newLobby);
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

    public bool lobbyListsEqual(List<Lobby> list1, List<Lobby> list2)
    {
        if (list1 != null && list2 != null)
        {
            return false;
        }
        for (int i = 0; i < list2.Count; i++)
        {
            if (list1[i].id == list2[i].id)
            {
                continue;
            }
            return false;
        }
        return true;
    }
}
