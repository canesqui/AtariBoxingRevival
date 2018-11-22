using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp1
{
    public class HandleConnection
    {
        private readonly TcpClient client;

        public HandleConnection(TcpClient client)
        {
            this.client = client;
        }

        public void Start()
        {

            var thread = new Thread(Handle);
            thread.Start();
        }
        
        private void Handle()
        {            
            while (client.Connected)
            {                
                try
                {
                    string bufferData = Util.GetBufferData(client);
                    if (bufferData != null)
                    {                        
                        Console.WriteLine("Received request at: " + DateTime.Now.ToUniversalTime().ToString());
                        Console.WriteLine("Payload received: "+ bufferData + "Size:" + bufferData.Length);
                        
                        Util.CreateTKSGRequest(client.GetStream(), bufferData);                        
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
