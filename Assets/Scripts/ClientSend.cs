using System.Net.NetworkInformation;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void createLobbyRequest(Player player)
    {
        using (Packet _packet = new Packet((int)ClientPackets.createLobbyReceived))
        {
            _packet.Write(player.username);
            _packet.Write(player.ip.ToString());

            SendTCPData(_packet);
        }
    }

    public static void joinLobbyRequest(string lobbyId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.lobbyJoinReceived))
        {
            _packet.Write(Client.player.ip.ToString());
            _packet.Write(lobbyId);

            SendTCPData(_packet);
        }
    }

    public static void playerNameRequestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerNameReceived))
        {
            string _username = UIManager.instance.usernameField.text;

            _packet.Write(_username);

            SendUDPData(_packet);
        }


    }

    public static void chatMessageReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.chatMessageReceived))
        {
            string _username = UIManager.instance.usernameField.text;

            string _message = UIManager.instance.chatInput.text;

            string _lobbyId = Client.player.currentLobby;

            _packet.Write(_username);

            _packet.Write(_message);

            _packet.Write(_lobbyId);

            SendTCPData(_packet);
        }
    }

    //public static void playerMacSend()
    //{
    //    using (Packet _packet = new Packet((int)ClientPackets.playerMacReceived))
    //    {
    //        PhysicalAddress mac = Client.player.mac;
    //
    //        _packet.Write(mac.ToString());
    //
    //        SendTCPData(_packet);
    //    }
    //}
    #endregion
}
