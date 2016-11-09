using System;
using System.Threading.Tasks;
using Flexylexy.Web.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Flexylexy.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ITokenizer _tokenizer;

        public ChatHub(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        [HubMethodName("SendChat")]
        public void SendChat(ChatMessage message)
        {
            if (String.IsNullOrEmpty(message.ConnectionToken))
            {
                Clients.All.ChatReceived(message.Content, message.SenderName);
            }
            else
            {
                var connectionId = _tokenizer.GetData(message.ConnectionToken);
                Clients.Client(connectionId).ChatReceived(message.Content, message.SenderName);
            }
        }

        public override Task OnConnected()
        {
            var token = _tokenizer.CreateToken(Context.ConnectionId);
            //Clients.Client(Context.ConnectionId).TokenGenerated(token);

            return base.OnConnected();
        }
    }
}