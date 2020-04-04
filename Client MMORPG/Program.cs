using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Client_MMORPG
{
    class Program
    {
        public static void thread1(NetworkStream stream)
        {
            while (true)
            {
                byte[] data2 = new byte[256];
                String responeseData = String.Empty;
                int bytes = stream.Read(data2, 0, data2.Length);
                responeseData = System.Text.Encoding.UTF8.GetString(data2, 0, bytes);
                int length = 1 + (responeseData.Length / Console.BufferWidth);
                Console.CursorTop -= length;
                Console.CursorLeft = 0;
                Console.WriteLine(new string(' ', responeseData.Length));
                Console.CursorTop -= length;
                Console.WriteLine(responeseData);
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Jannes Chatt Program";
            Console.WriteLine("type IP");
            string ip = Console.ReadLine();

            Console.WriteLine("type port");
            string portString = Console.ReadLine();
            bool success = int.TryParse(portString, out int port);

            Console.WriteLine("Type username");
            string username = Console.ReadLine();
            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
            Thread t = new Thread(() => thread1(stream));
            t.Start();
            Console.WriteLine("You can now begin communicating");
            while (true)
            {
              String messeage = username + ": " +  Console.ReadLine();
              Byte[] data = System.Text.Encoding.UTF8.GetBytes(messeage);
              stream.Write(data, 0, data.Length);
            }
        }
    }
}
