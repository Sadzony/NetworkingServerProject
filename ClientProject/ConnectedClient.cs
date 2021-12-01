using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace ServerProject
{
    class ConnectedClient
    {
        private Socket m_socket;
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        private BinaryFormatter m_formatter;
        private object m_readLock;
        private object m_writeLock;
        public string m_name;
        public IPEndPoint m_EndPoint;
        public ConnectedClient(Socket p_socket)
        {
            m_writeLock = new object();
            m_readLock = new object();
            m_socket = p_socket;
            m_stream = new NetworkStream(m_socket, true);
            m_reader = new BinaryReader(m_stream, Encoding.UTF8);
            m_writer = new BinaryWriter(m_stream, Encoding.UTF8);
            m_formatter = new BinaryFormatter();

        }
        public void Close()
        {
            m_reader.Close();
            m_writer.Close();
            m_stream.Close();
            m_socket.Close();
        }
        public Packets.Packet Read_tcp() //read from client side
        {
            lock (m_readLock)
            {
                int numberOfBytes;
                if((numberOfBytes = m_reader.ReadInt32()) != -1)
                {
                    byte[] buffer = m_reader.ReadBytes(numberOfBytes);
                    MemoryStream memoryStream = new MemoryStream(buffer);
                    return m_formatter.Deserialize(memoryStream) as Packets.Packet;
                }
                else
                {
                    return null;
                }
            }

        }
        public void Send_tcp(Packets.Packet packet) //send message to client side
        {
            lock (m_writeLock)
            {
                MemoryStream memoryStream = new MemoryStream();
                m_formatter.Serialize(memoryStream, packet);
                byte[] buffer = memoryStream.GetBuffer();
                m_writer.Write(buffer.Length);
                m_writer.Write(buffer);
                m_writer.Flush();
            }
        }

    }
}
