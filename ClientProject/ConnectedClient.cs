using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ServerProject
{
    class ConnectedClient
    {
        private Socket m_socket;
        private NetworkStream m_stream;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        private object m_readLock;
        private object m_writeLock;
        public ConnectedClient(Socket p_socket)
        {
            m_writeLock = new object();
            m_readLock = new object();
            m_socket = p_socket;
            m_stream = new NetworkStream(m_socket, true);
            m_reader = new StreamReader(m_stream, Encoding.UTF8);
            m_writer = new StreamWriter(m_stream, Encoding.UTF8);

        }
        public void Close()
        {
            m_stream.Close();
            m_reader.Close();
            m_writer.Close();
            m_socket.Close();
        }
        public string Read()
        {
            lock (m_readLock)
            {
                return m_reader.ReadLine();
            }

        }
        public void Send(string message)
        {
            lock (m_writeLock)
            {
                m_writer.WriteLine(message);
                m_writer.Flush();
            }
        }

    }
}
