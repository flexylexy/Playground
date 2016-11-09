using System.Collections.Generic;

namespace Flexylexy.Web.Models
{
    //public interface IRoster
    //{
    //    IEnumerable<IClient> ConnectedClients { get; }

    //    void Add(IClient client);

    //    void Remove(string connectionToken);

    //    void Clear();
    //}

    public interface IRoster : IDictionary<string, IClient>
    {
        IEnumerable<IClient> ConnectedClients { get; }
    }
}