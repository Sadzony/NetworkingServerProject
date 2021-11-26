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
        private ConcurrentDictionary<int, ConnectedClient> m_clients;
        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_tcpListener = new TcpListener(ip, port);

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
                    Thread thread = new Thread(() => { ClientMethod(index); });
                    thread.Start();
                }
            }
        }
        public void Stop()
        {
            m_tcpListener.Stop();

        }
        private void ClientMethod(int index)
        {
            string receivedMessage;


            m_clients[index].Send("You have connected to the server - send 0 for valid options");
            while((receivedMessage = m_clients[index].Read()) != null)
            {
                m_clients[index].Send(GetReturnMessage(receivedMessage));
                Console.WriteLine("Received message!");
                if (GetReturnMessage(receivedMessage) == "bye")
                {
                    break;
                }

            }
            m_clients[index].Close();
            ConnectedClient c;
            m_clients.TryRemove(index, out c);
            clientIndex--;
            Console.WriteLine("Client Disconnected.");

        }
        private string GetReturnMessage(string code)
        {
            if(code == "exit")
            {
                return "bye";
                
            }
            return "i hear you!";
        }
    }
}
