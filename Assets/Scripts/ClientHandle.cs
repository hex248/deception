using System.Collections;
using System.Collections.Generic;
using System.Net;
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

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void playerName(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {_msg}");
        ClientSend.playerNameRequestReceived();
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

        Debug.Log($"Received packet via UDP. Contains chat message: {_message} from {_username}");
        ClientSend.chatMessageReceived();
    }
}
