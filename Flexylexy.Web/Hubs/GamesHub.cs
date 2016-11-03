using System.Threading.Tasks;
using Flexylexy.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Flexylexy.Web.Hubs
{
    public partial class GamesHub : Hub
    {
        [HubMethodName("Challenge")]
        public void Challenge(string name, string opponentConnectionId)
        {
            Clients.Client(opponentConnectionId).Challenged(name, Context.ConnectionId);
        }

        [HubMethodName("AcceptChallenge")]
        public void AcceptChallenge(string opponentConnectionId)
        {
            Clients.Client(opponentConnectionId).ChallengeAccepted(Context.ConnectionId);
        }

        [HubMethodName("DeclineChallenge")]
        public void DeclineChallenge(string opponentConnectionId)
        {
            Clients.Client(opponentConnectionId).ChallengeDeclined();
        }

        [HubMethodName("AddClient")]
        public void AddClient(string name)
        {
            Roster.AddClient(name, Context.ConnectionId);
            Clients.All.PlayersUpdated(Roster.ConnectedPlayers);
        }

        [HubMethodName("GetPlayers")]
        public void GetPlayers()
        {
            Clients.Client(Context.ConnectionId).PlayersUpdated(Roster.ConnectedPlayers);
        }

        [HubMethodName("ClearPlayers")]
        public void ClearPlayers()
        {
            Roster.ClearPlayers();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Roster.RemoveClient(Context.ConnectionId);
            Clients.All.PlayersUpdated(Roster.ConnectedPlayers);

            return base.OnDisconnected(stopCalled);
        }
    }
}