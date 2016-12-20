using System.Collections.Generic;
using System.Linq;

namespace Flexylexy.Web.Models
{
    public class Roster : IRoster
    {
        private readonly ITokenizer _tokenizer;
        private readonly IDictionary<string, IClient> _clientsByToken = new Dictionary<string, IClient>();
        private readonly IDictionary<string, IClient> _clientsByConnectionId = new Dictionary<string, IClient>();

        public IEnumerable<IClient> ConnectedClients => _clientsByToken.Values;

        public Roster(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public IClient ByConnectionId(string connectionId)
        {
            return _clientsByConnectionId[connectionId];
        }

        public IClient ByToken(string token)
        {
            return _clientsByToken[token];
        }

        public void Add(string connectionId, IClient client)
        {
            _clientsByToken.Add(client.ConnectionToken, client);
            _clientsByConnectionId.Add(connectionId, client);
        }

        public void Remove(IClient client)
        {
            RemoveByToken(client.ConnectionToken);
        }

        public void RemoveByConnectionId(string connectiondId)
        {
            _clientsByConnectionId.Remove(connectiondId);
            var token = _tokenizer.CreateToken(connectiondId);
            _clientsByToken.Remove(token);
        }

        public void RemoveByToken(string token)
        {
            _clientsByToken.Remove(token);
            var connectionId = _tokenizer.GetData(token);
            _clientsByConnectionId.Remove(connectionId);
        }

        public void Clear()
        {
            _clientsByToken.Clear();
            _clientsByConnectionId.Clear();
        }
    }
}