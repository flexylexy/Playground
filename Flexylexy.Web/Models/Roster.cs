using System.Collections.Generic;
using System.Linq;
using Flexylexy.Web.Models;

namespace Flexylexy.Web.Models
{
    public static class Roster
    {
        private static readonly IList<Player> _connectedPlayers = new List<Player>();
        public static IEnumerable<Player> ConnectedPlayers => _connectedPlayers;

        public static void AddClient(string name, string connectionId)
        {
            _connectedPlayers.Add(new Player { Name = name, ConnectionId = connectionId });
        }

        public static void RemoveClient(string connectionId)
        {
            _connectedPlayers.Remove(_connectedPlayers.FirstOrDefault(x => x.ConnectionId == connectionId));
        }

        public static void ClearPlayers()
        {
            _connectedPlayers.Clear();
        }
    }
}