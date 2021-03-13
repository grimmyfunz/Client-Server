using System;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    class Program
    {
        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static void Main(string[] args)
        {
            socket.Connect("localhost", 8008);
            string message = Console.ReadLine();
            socket.Send(GetBuffer(message));
            Console.ReadLine();
        }

        static byte[] GetBuffer(string message)
        {
            return Encoding.ASCII.GetBytes(message);
        }
    }
}
