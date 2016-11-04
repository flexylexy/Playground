using System.Collections.Generic;
using System.Linq;

namespace Flexylexy.Web.Models
{
    public class Roster : IRoster
    {
        private readonly IList<IClient> _connectedClients = new List<IClient>();
        public IEnumerable<IClient> ConnectedClients => _connectedClients;
        
        public void Add(IClient client)
        {
            _connectedClients.Add(client);
        }

        public void Remove(string connectionToken)
        {
            _connectedClients.Remove(_connectedClients.FirstOrDefault(x => x.ConnectionToken == connectionToken));
        }

        public void Clear()
        {
            _connectedClients.Clear();
        }
    }
}