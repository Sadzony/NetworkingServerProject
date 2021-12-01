using System;
using System.Net;
namespace Packets
{
    public enum PacketType
    {
        ChatMessage,
        PrivateMessage,
        ClientName,
        Login
    }
    [Serializable()]
    public class Packet
    {
        protected PacketType packetType;
        public PacketType GetPacketType()
        {
            return packetType;
        }
        protected void SetType(PacketType ptype)
        {
            packetType = ptype;
        }
    }
    [Serializable()]
    public class ChatMessagePacket : Packet
    {
        public string message;
        public ChatMessagePacket(string p_message)
        {
            message = p_message;
            SetType(PacketType.ChatMessage);
        }
    }
    public class LoginPacket : Packet
    {
        public IPEndPoint mEndPoint;
        public LoginPacket(IPEndPoint pEndPoint)
        {
            mEndPoint = pEndPoint;
            SetType(PacketType.Login);

        }
    }
}
