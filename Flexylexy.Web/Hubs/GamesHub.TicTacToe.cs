using Microsoft.AspNet.SignalR.Hubs;

namespace Flexylexy.Web.Hubs
{
    public partial class GamesHub
    {
        [HubMethodName("Play")]
        public void Play(int position, string opponentConnectionId)
        {
            Clients.Client(opponentConnectionId).Play(Context.ConnectionId, position);
        }
    }
}