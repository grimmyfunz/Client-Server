using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestServer
//{
//    class Program
//    {
//        static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//        static byte[] buffer = new byte[1024];

//        static void Main(string[] args)
//        {
//            socket.Bind(new IPEndPoint(IPAddress.Any, 8008));
//            socket.Listen(1024);


//            while (true)
//            {
//                Socket clientSocket = socket.Accept();
//                Console.WriteLine($"Client have been connected!");
//                clientSocket.Receive(buffer);
//                Console.WriteLine(Encoding.ASCII.GetString(buffer));
//                Console.ReadLine();
//            }
//        }

//        static byte[] GetBuffer(string message)
//        {
//            return Encoding.ASCII.GetBytes(message);
//        }
//    }
//}

{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

            int count = 1;

            TcpListener ServerSocket = new TcpListener(IPAddress.Any, 8008);
            ServerSocket.Start();

            while (true)
            {
                TcpClient client = ServerSocket.AcceptTcpClient();
                list_clients.Add(count, client);
                Console.WriteLine("User connected");
                count++;
                Box box = new Box(client, list_clients);

                Thread t = new Thread(handle_clients);
                t.Start(box);
            }

        }

        public static void handle_clients(object o)
        {
            Box box = (Box)o;
            Dictionary<int, TcpClient> list_connections = box.list;

            while (true)
            {
                NetworkStream stream = box.c.GetStream();
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                byte[] formated = new Byte[byte_count];
                //handle  the null characteres in the byte array
                Array.Copy(buffer, formated, byte_count);
                string data = Encoding.ASCII.GetString(formated);
                broadcast(list_connections, data);
                Console.WriteLine(data);

            }
        }

        public static void broadcast(Dictionary<int, TcpClient> conexoes, string data)
        {
            foreach (TcpClient c in conexoes.Values)
            {
                NetworkStream stream = c.GetStream();

                byte[] buffer = Encoding.ASCII.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

    }
    class Box
    {
        public TcpClient c;
        public Dictionary<int, TcpClient> list;

        public Box(TcpClient c, Dictionary<int, TcpClient> list)
        {
            this.c = c;
            this.list = list;
        }

    }
}