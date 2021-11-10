using System;


namespace NetworkingServerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            if(client.Connect("127.0.0.1", 4444))
            {
                client.Run();
            }
            else
            {
                Console.WriteLine("Failed to connect to server.");
            }
        }
    }
}
