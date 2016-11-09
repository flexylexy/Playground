using System.Collections.Generic;
using System.Linq;

namespace Flexylexy.Web.Models
{
    public class Roster : Dictionary<string, IClient>, IRoster
    {
        //private readonly IList<IClient> _connectedClients = new List<IClient>();
        //public IEnumerable<IClient> ConnectedClients => _connectedClients;

        //public IClient this[string conn]
        //{
        //    get
        //    {
        //        return _connectedClients.FirstOrDefault(x =? )
        //    }
        //}
        
        //public void Add(IClient client)
        //{
        //    _connectedClients.Add(client);
        //}

        //public void Remove(string connectionToken)
        //{
        //    _connectedClients.Remove(_connectedClients.FirstOrDefault(x => x.ConnectionToken == connectionToken));
        //}

        //public void Clear()
        //{
        //    _connectedClients.Clear();
        //}

        public IEnumerable<IClient> ConnectedClients => Values;
    }
}