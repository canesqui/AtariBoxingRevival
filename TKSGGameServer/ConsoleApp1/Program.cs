using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using TKSGGameServer;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;

            //Initializing dummy registered player list - it would come from database 
            //and should have a way to register in real world application
            RegisteredPlayers registeredPlayers = new RegisteredPlayers();
            PlayerQueue playerQueue = new PlayerQueue();
            List<OngoingMatch> ongoingMatches = new List<OngoingMatch>();
            List<Score> scores = new List<Score>();

            registeredPlayers.Add(new Player() { Name = "Ronaldo Canesqui", Email = "canesqui@gmail.com", Token= "GFZ+oji6jUWy4WzsMswesw==" });
            registeredPlayers.Add(new Player() { Name = "Tiago Tel", Email = "something@gmail.com", Token = "GMqsqw/FxEqrwFqQWGUGFg==" });
            
            int port;

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out port))
                    throw new ArgumentException("The port parameter was not provided correctly. The port must be a valid integer value.");
            }
            else
            {
                port = 8888;
            }

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var ip = Util.GetIpAddresses();

                IPAddress localAddr = IPAddress.Parse(ip.ToString());

                server = new TcpListener(localAddr, port);

                server.Start();
                Console.WriteLine("Game server initialized");
                Console.WriteLine("Waiting for connections on port {0}", port);
                Console.WriteLine("Server IP address: {0}", ip.ToString());                
                Console.WriteLine();
                Console.WriteLine("You can change these configuration on VS project's properties window on Debug");
                Console.WriteLine("and use {0} as the command line argument field or", "PORT");
                Console.WriteLine("you can use TKSGGameServer {0} from the command line.", "PORT");
                Console.WriteLine();

                while (true)
                {

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");                    

                    var connectionThread = new HandleConnection(client, registeredPlayers, playerQueue, ongoingMatches, scores);

                    connectionThread.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}