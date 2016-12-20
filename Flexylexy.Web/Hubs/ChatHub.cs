using Flexylexy.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Flexylexy.Web.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        private readonly ITokenizer _tokenizer;
        private readonly IRoster _roster;

        public ChatHub(IRoster roster, ITokenizer tokenizer)
        {
            _roster = roster;
            _tokenizer = tokenizer;
        }

        [HubMethodName("SendChat")]
        public void SendChat(string message, Client client)
        {
            if (client == null)
            {
                //foreach (var c in _roster.ConnectedClients)
                //{
                //    var connectionId = _tokenizer.GetData(c.ConnectionToken);
                //    Clients.Client(connectionId).ChatMe(message);
                //}

                //Clients.Client(Context.ConnectionId).ChatMe(message);

                Clients.All.ChatReceived(message, _roster.ByConnectionId(Context.ConnectionId));
                //GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients.All.ChatMe(message);
            }
            else
            {
                var connectionId = _tokenizer.GetData(client.ConnectionToken);
                Clients.Client(connectionId).ChatReceived(message, _roster.ByConnectionId(Context.ConnectionId));
            }
        }
    }
}