using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;

        ClientSend.WelcomeReceived();
        //string macAddresses = "";
        //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        //{
        //    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
        //    {
        //        macAddresses += nic.GetPhysicalAddress().ToString();
        //        break;
        //    }
        //}
        //Debug.Log($"Mac Addresses: {macAddresses}");
        //try
        //{
        //    Client.player.mac = PhysicalAddress.Parse(macAddresses);
        //}
        //finally
        //{
        //    Client.player.mac = PhysicalAddress.Parse("7A791953A077");
        //}
        

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void playerName(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {_msg}");
        ClientSend.playerNameRequestReceived();
    }

    public static void PlayerObject(Packet _packet)
    {
        string ip = _packet.ReadString();
        string username = _packet.ReadString();

        Client.player.ip = ip;
        Client.player.username = username;

        Debug.Log($"Received player object packet via TCP. Contains ip: {ip} Contains username: {username}");
        //ClientSend.playerMacSend();
    }

    public static void lobbyAccepted(Packet _packet)
    {
        string lobbyJoinedId = _packet.ReadString();

        Client.player.currentLobby = lobbyJoinedId;

        Debug.Log($"Joined lobby: {Client.player.currentLobby}");

        UIManager.LobbyWaitingRoomStaticOBJ.SetActive(true);
        UIManager.lobbySelectionStaticOBJ.SetActive(false);
    }

    public static void lobbyUpdate(Packet _packet)
    {
        Lobby lobby = new Lobby();

        lobby.id = _packet.ReadString();
        lobby.ownerIP = _packet.ReadString();
        lobby.ownerName = _packet.ReadString();
        lobby.playerCount = _packet.ReadInt();
        string updateType = _packet.ReadString();

        bool isNew = true;
        foreach (Lobby storedLobby in Client.lobbyList)
        {
            if (storedLobby.id == lobby.id)
            {
                isNew = false;
            }
        }


        if (updateType == "clear")
        {
            Debug.Log($"{lobby.id} is going to be cleared");
            foreach (Lobby lobbyTwo in Client.lobbyList)
            {
                if (lobbyTwo.id == lobby.id)
                {
                    Client.lobbyList.Remove(lobbyTwo);
                }
            }
        }
        else if (isNew)
        {
            Client.lobbyList.Add(lobby);
        }

        UIManager.instance.RefreshLobbies();
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Debug.Log($"client with id {_id} disconnected");
    }

    public static void chatMessage(Packet _packet)
    {
        string _username = _packet.ReadString();
        string _message = _packet.ReadString();

        Debug.Log($"(TCP) Chat message received: {_message} from {_username}");

        UIManager.SendMessageToChat(_username, _message);
    }
}