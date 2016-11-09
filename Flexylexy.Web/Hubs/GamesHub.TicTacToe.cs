using Flexylexy.Web.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace Flexylexy.Web.Hubs
{
    public partial class GamesHub
    {
        [HubMethodName("Play")]
        public void Play(Move move)
        {
            var opponentConnectionId = _tokenizer.GetData(move.ConnectionToken);
            move.ConnectionToken = _tokenizer.CreateToken(Context.ConnectionId);
            Clients.Client(opponentConnectionId).Play(move);
        }
    }
}