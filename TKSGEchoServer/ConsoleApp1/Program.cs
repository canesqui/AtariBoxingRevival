using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            
            int port;

            if (args.Length > 1)
            {
                if (!int.TryParse(args[1], out port))
                    throw new ArgumentException("The port parameter was not provided correctly. The port must be a valid integer value.");
            }
            else
            {
                port = 8889;
            }

            try
            {

                var ip = Util.GetIpAddresses();

                IPAddress localAddr = IPAddress.Parse(ip.ToString());

                server = new TcpListener(localAddr, port);

                server.Start();
                Console.WriteLine("TKSG 10 Echo Game server initialized");
                Console.WriteLine("Waiting for connections on port {0}", port);
                Console.WriteLine("Server IP address: {0}", ip.ToString());                
                Console.WriteLine();
                Console.WriteLine("You can change this configuration on VS project's properties window on Debug");     
                Console.WriteLine("you can use ThanksgivingGameServer {0} from the command line.", "PORT");
                Console.WriteLine();

                while (true)
                {

                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");                    

                    var connectionThread = new HandleConnection(client);

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