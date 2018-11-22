using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TKSGGameServer
{
    public class Player
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public TcpClient TcpClient { get; set; }
    }
}
