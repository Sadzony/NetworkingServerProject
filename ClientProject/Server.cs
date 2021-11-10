using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ClientProject
{
    class Server
    {
        private TcpListener m_tcpListener;
        public Server(string ipAddress, int port)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            m_tcpListener = new TcpListener(ip, port);

        }
        public void Start()
        {
            m_tcpListener.Start();
            Socket socket = m_tcpListener.AcceptSocket();
            Console.WriteLine("Connection made");
            ClientMethod(socket);
        }
        public void Stop()
        {
            m_tcpListener.Stop();

        }
        private void ClientMethod(Socket socket)
        {
            string receivedMessage;
            NetworkStream stream = new NetworkStream(socket, true);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine("You have connected to the server - send 0 for valid options");
            writer.Flush();
            while((receivedMessage = reader.ReadLine()) != null)
            {
                writer.WriteLine(GetReturnMessage(receivedMessage));
                Console.WriteLine("Received message!");
                writer.Flush();
            }
            socket.Close();
            
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
