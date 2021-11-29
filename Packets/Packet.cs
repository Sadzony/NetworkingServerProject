using System;
namespace Packets
{
    public enum PacketType
    {
        ChatMessage,
        PrivateMessage,
        ClientName
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
}
