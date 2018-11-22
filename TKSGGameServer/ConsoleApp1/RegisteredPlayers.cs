using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKSGGameServer
{
    public class RegisteredPlayers
    {
        //private IQueryable<Player> registeredPlayers = new IQueryable<Player>(); 
        private List<Player> registeredPlayers = new List<Player>();
        public void Add(Player player) {
            lock (registeredPlayers) {
                registeredPlayers.Add(player);
            }
        }

        public List<Player> Get()
        {
            return registeredPlayers;
        }
    }
}
