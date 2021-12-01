using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
namespace ServerProject
{
     
    class Server
    {
        private int clientIndex = 0;
        private TcpListener m_tcpListener;
        private UdpClient m_udpListener;

        private ConcurrentDictionary<int, ConnectedClient> m_clients;
        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_tcpListener = new TcpListener(ip, port);
            m_udpListener = new UdpClient(port);

        }
        public void Start()
        {
            m_clients = new ConcurrentDictionary<int, ConnectedClient>();
            
            m_tcpListener.Start();
            while (true)
            {
                if (clientIndex <= 2)
                {
                    Socket socket = m_tcpListener.AcceptSocket();
                    ConnectedClient new_client = new ConnectedClient(socket);
                    int index = clientIndex;
                    clientIndex++;
                    m_clients.TryAdd(index, new_client);
                    Console.WriteLine("Connection made");
                    Thread thread = new Thread(() => { ClientMethod_tcp(index); });
                    thread.Start();
                    Thread udpThread = new Thread(() => { UdpListen(); });
                    udpThread.Start();
                }
                else
                {
                    Socket socket = m_tcpListener.AcceptSocket();
                    Console.WriteLine("Connection made. Server is full");
                    ConnectedClient new_client = new ConnectedClient(socket);
                    new_client.Close();
                    Console.WriteLine("Client Disconnected.");
                }
            }
        }
        public void Stop()
        {
            m_tcpListener.Stop();

        }
        private void ClientMethod_tcp(int index)
        {
            Packets.Packet receivedMessage;

            while((receivedMessage = m_clients[index].Read_tcp()) != null)
            {
                switch (receivedMessage.GetPacketType())
                {
                    case Packets.PacketType.ChatMessage:
                        Console.WriteLine("Received message!");
                        Packets.ChatMessagePacket chatPacket = (Packets.ChatMessagePacket)receivedMessage;
                        m_clients[index].Send_tcp(new Packets.ChatMessagePacket(chatPacket.message));
                        break;
                    case Packets.PacketType.Login:
                        m_clients[index].m_EndPoint = ((Packets.LoginPacket)receivedMessage).mEndPoint;
                        break;
                    case Packets.PacketType.ClientName:
                        break;
                    case Packets.PacketType.PrivateMessage:
                        break;
                }
                

            }
            m_clients[index].Close();
            ConnectedClient c;
            m_clients.TryRemove(index, out c);
            clientIndex--;
            Console.WriteLine("Client Disconnected.");

        }
        private void UdpListen()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                while (m_udpListener != null)
                {
                    byte[] buffer = m_udpListener.Receive(ref endPoint);
                    foreach(ConnectedClient c in m_clients.Values)
                    {
                        if(endPoint.ToString() == c.m_EndPoint.ToString()) //find which client sent the packet
                        {

                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Client UDP Read Method Exception: " + e.Message);
            }
        }
    }
}
