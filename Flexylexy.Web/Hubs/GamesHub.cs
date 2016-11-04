using System;
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
        public void Challenge(string name, string opponentConnectionToken)
        {
            var opponentConnectionId = _tokenizer.GetData(opponentConnectionToken);
            Clients.Client(opponentConnectionId).Challenged(name, Context.ConnectionId);
        }

        [HubMethodName("AcceptChallenge")]
        public void AcceptChallenge(string opponentConnectionToken)
        {
            var opponentConnectionId = _tokenizer.GetData(opponentConnectionToken);
            Clients.Client(opponentConnectionId).ChallengeAccepted(Context.ConnectionId);
        }

        [HubMethodName("DeclineChallenge")]
        public void DeclineChallenge(string opponentConnectionToken)
        {
            var opponentConnectionId = _tokenizer.GetData(opponentConnectionToken);
            Clients.Client(opponentConnectionId).ChallengeDeclined();
        }

        [HubMethodName("AddClient")]
        public void AddClient(string name)
        {
            var token = _tokenizer.CreateToken(Context.ConnectionId);
            _roster.Add(new Client { Name = name, ConnectionToken = token });
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

        public void SendChat(string message, string sender, string connectionId)
        {
            if (String.IsNullOrEmpty(connectionId))
            {
                Clients.All.ReceiveChat(message, sender);
            }
            else
            {
                Clients.Client(connectionId).ReceiveChat(message, sender);
            }
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