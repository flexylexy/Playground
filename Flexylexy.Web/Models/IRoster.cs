using System.Collections.Generic;

namespace Flexylexy.Web.Models
{
    public interface IRoster
    {
        IEnumerable<IClient> ConnectedClients { get; }
        IClient ByConnectionId(string connectionId);
        IClient ByToken(string token);
        void Add(string connectionId, IClient client);
        void Remove(IClient client);
        void RemoveByConnectionId(string connectiondId);
        void RemoveByToken(string token);
        void Clear();
    }
}