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

            string _message = UIManager.instance.chatBox.text;

            _packet.Write(_username);

            _packet.Write(_message);

            SendUDPData(_packet);
        }


    }
    #endregion
}
