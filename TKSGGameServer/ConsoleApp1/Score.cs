using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKSGGameServer
{
    public class Score
    {
        public string MatchToken { get; set; }
        public string Player1Token { get; set; }
        public int Player1Score { get; set; }
        public string Player2Token { get; set; }
        public int Player2Score { get; set; }
    }
}
