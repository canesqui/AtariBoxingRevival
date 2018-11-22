using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKSGGameServer
{
    public class PlayerQueue
    {
        private Queue<Player> waitingList = new Queue<Player>();

        public void Add(Player player)
        {
            lock (waitingList) {
                waitingList.Enqueue(player);
            }
        }

        public Player Dequeue()
        {
            lock (waitingList) {
                return waitingList.Dequeue();
            }
        }

        public int Count { get { return waitingList.Count; } }
    }
}
