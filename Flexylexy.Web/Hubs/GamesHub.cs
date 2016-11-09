using System.Threading.Tasks;
using Flexylexy.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Flexylexy.Web.Hubs
{
    public partial class GamesHub : Hub
    {
        private readonly IRoster _roster;
        private readonly ITokenizer _tokenizer;

        public GamesHub(IRoster roster, ITokenizer tokenizer)
        {
            _roster = roster;
            _tokenizer = tokenizer;
        }

        [HubMethodName("Challenge")]
        public void Challenge(Client client)
        {
            var opponentConnectionId = _tokenizer.GetData(client.ConnectionToken);
            var token = _tokenizer.CreateToken(Context.ConnectionId);

            Clients.Client(opponentConnectionId).Challenged(_roster[token]);
        }

        [HubMethodName("AcceptChallenge")]
        public void AcceptChallenge(Client client)
        {
            var opponentConnectionId = _tokenizer.GetData(client.ConnectionToken);
            var token = _tokenizer.CreateToken(Context.ConnectionId);

            Clients.Client(opponentConnectionId).ChallengeAccepted(_roster[token]);
        }

        [HubMethodName("DeclineChallenge")]
        public void DeclineChallenge(Client client)
        {
            var opponentConnectionId = _tokenizer.GetData(client.ConnectionToken);
            Clients.Client(opponentConnectionId).ChallengeDeclined();
        }

        [HubMethodName("AddClient")]
        public void AddClient(string name)
        {
            var token = _tokenizer.CreateToken(Context.ConnectionId);
            _roster.Add(token, new Client { Name = name, ConnectionToken = token });
            Clients.All.PlayersUpdated(_roster.ConnectedClients);
        }

        [HubMethodName("GetPlayers")]
        public void GetPlayers()
        {
            Clients.Client(Context.ConnectionId).PlayersUpdated(_roster.ConnectedClients);
        }

        [HubMethodName("ClearPlayers")]
        public void ClearPlayers()
        {
            _roster.Clear();
        }

        public override Task OnConnected()
        {
            var token = _tokenizer.CreateToken(Context.ConnectionId);
            Clients.Client(Context.ConnectionId).TokenGenerated(token);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var token = _tokenizer.CreateToken(Context.ConnectionId);
            _roster.Remove(token);
            Clients.All.PlayersUpdated(_roster.ConnectedClients);

            return base.OnDisconnected(stopCalled);
        }
    }
}