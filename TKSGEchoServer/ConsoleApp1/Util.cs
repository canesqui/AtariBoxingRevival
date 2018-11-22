using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Util
    {
        public static IPAddress GetIpAddresses()
        {
            IPAddress result = null;
            String strHostName = Dns.GetHostName();
            
            IPHostEntry iphostentry = Dns.GetHostEntry(strHostName);
            foreach (var item in iphostentry.AddressList)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = item;
                }
            }
            return result;
        }

        public static string GetBufferData(TcpClient tcpClient)
        {
            StringBuilder dataReceived = new StringBuilder();
            if (tcpClient.GetStream().CanRead)
            {
                byte[] myReadBuffer = new byte[9];
                int numberOfBytesRead = 0;


                do
                {
                    numberOfBytesRead = tcpClient.GetStream().Read(myReadBuffer, 0, myReadBuffer.Length);

                    dataReceived.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                }
                while (tcpClient.GetStream().DataAvailable);
            }

            return dataReceived.ToString();
        }
        
        public static void CreateTKSGRequest(NetworkStream stream, string payload)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);
            writer.Write(payload);            
            writer.Flush();
        }        
    }
}
