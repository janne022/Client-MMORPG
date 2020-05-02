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
                /*this while loop uses stream.Read to get data from the server, the stream we are using is the server and by reading it
                 * we can get the bytes coming from the stream and then we can use the GetString to convert bytes to a string, which we then
                 * print to the user
                */
                while (true)
                {
                    byte[] data2 = new byte[256];
                    string responeseData = string.Empty;
                    int bytes = stream.Read(data2, 0, data2.Length);
                    responeseData = System.Text.Encoding.UTF8.GetString(data2, 0, bytes);
                    Console.WriteLine(responeseData);
                }
            }
            //if the server closes down for some reason, it will deliver this message to you and return you to the start.
            catch (Exception)
            {
                Console.WriteLine("It looks the the connection with the server broke");
                return;
            }
        }

        public static void Start()
        {
            //just empty variables and title for program
            string ip;
            int port;
            Console.Title = "Jannes Chatt Program";
            while (true)
            {
                /*Asks for IP and Port, then parses the port to an int. We do this because the TcpClient wants the ip/hostname as a string
                 * and then port as an int.
                */ 
                Console.WriteLine("type IP");
                ip = Console.ReadLine();

                Console.WriteLine("type port");
                string portString = Console.ReadLine();
                bool success = int.TryParse(portString, out port);
                /*This trycatch is a test for if it can actually connect to the server, if it fails it will notify the user and 
                 *go back to the start of the loop.
                */
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
            //Connects to the server and asks user for the username and password, it then sends this information to the server.
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
            //This starts a thread. It uses lambda expressions and this is the thread that writes out all information coming from the server.
            Thread liveChat = new Thread(() => LiveChat(stream));
            liveChat.Start();
            Console.WriteLine("You can now begin communicating");
            //Now the user can type his own messages and anything he says is converted to Bytes and sent using the stream.Write() function
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
        //Main method just starts the program and the while loop is to ensure that if a trycatch boots a player out from the program, it will start it again.
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