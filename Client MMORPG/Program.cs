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
        //This function prints messages from server. This means that it prints messages sent from users.
        public static void LiveChat(NetworkStream stream)
        {
            try
            {
                while (true)
                {
                    byte[] data2 = new byte[256];
                    string responeseData = string.Empty;
                    int bytes = stream.Read(data2, 0, data2.Length);
                    responeseData = System.Text.Encoding.UTF8.GetString(data2, 0, bytes);
                    Console.WriteLine(responeseData);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("It looks the the connection with the server broke");
                return;
            }
        }

        public static void Start()
        {
            string ip;
            int port;
            Console.Title = "Jannes Chatt Program";
            //Checks if the ip and port typed by user is working, does not allow user out of loop until connection works
            while (true)
            {
                Console.WriteLine("type IP");
                ip = Console.ReadLine();

                Console.WriteLine("type port");
                string portString = Console.ReadLine();
                bool success = int.TryParse(portString, out port);
                try
                {
                    TcpClient testClient = new TcpClient(ip, port);
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Could not connect, doublecheck the ip/port and try again");
                }
            }
            TcpClient client = new TcpClient(ip, port);
            Console.Clear();
            Console.WriteLine("Connected");
            Console.WriteLine("Type username");
            string username = Console.ReadLine();
            Console.WriteLine("Type password");
            string password = Console.ReadLine();
            NetworkStream stream = client.GetStream();
            string credentials = ($"{username},{password}");
            Byte[] credentialsData = System.Text.Encoding.UTF8.GetBytes(credentials);
            try
            {
                stream.Write(credentialsData, 0, credentialsData.Length);
            }
            catch (Exception)
            {
                Console.WriteLine("It looks the the connection with the server broke");
                return;
            }




            Thread liveChat = new Thread(() => LiveChat(stream));
            liveChat.Start();
            Console.WriteLine("You can now begin communicating");
            try
            {
                while (true)
                {
                    string message = username + ": " + Console.ReadLine();
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("It looks the the connection with the server broke");
                return;
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Start();
            }
        }
    }
}