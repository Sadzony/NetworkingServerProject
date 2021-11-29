using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
namespace ClientProj
{
    public class Client
    {
        private TcpClient m_tcpClient;
        private NetworkStream stream;
        private BinaryWriter m_writer;
        private BinaryReader m_reader;
        private MainWindow m_form;
        private BinaryFormatter m_formatter;
        public Client()
        {
            m_tcpClient = new TcpClient();
        }
        
        public bool Connect(string ipAddress, int port)
        {
            try
            {
                m_tcpClient.Connect(ipAddress, port);
                stream = m_tcpClient.GetStream();
                m_reader = new BinaryReader(stream, Encoding.UTF8);
                m_writer = new BinaryWriter(stream, Encoding.UTF8);
                m_formatter = new BinaryFormatter();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }
        public void Run()
        {
            m_form = new MainWindow(this);
            Thread thread = new Thread(() => { ProcessServerResponse(); });
            m_form.ShowDialog();
        }
        public void SendMessage(Packets.Packet packet)
        {
            MemoryStream memoryStream = new MemoryStream();
            m_formatter.Serialize(memoryStream, packet);
            byte[] buffer = memoryStream.GetBuffer();
            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
            m_writer.Flush();
        }
        private Packets.Packet ProcessServerResponse()
        {
            int numberOfBytes;
            if ((numberOfBytes = m_reader.ReadInt32()) != -1)
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
}
