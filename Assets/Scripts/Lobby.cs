using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lobby : MonoBehaviour
{
    public string id;
    public string ownerIP;
    public string ownerName;
    public List<Player> players = new List<Player>();
    public int maxPlayers = 5;
    public int playerCount;

    public TextMeshProUGUI idObj;
    public TextMeshProUGUI usernameObj;
    public TextMeshProUGUI playerCountObj;

    public void join()
    {
        UIManager.instance.joinLobbyRequest(id);
    }
}