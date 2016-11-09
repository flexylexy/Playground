using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace Flexylexy.Web.Hubs
{
    public class DiHubDispatcher : HubDispatcher
    {
        private readonly Container _container;
        public DiHubDispatcher(HubConfiguration configuration, Container container)
            : base(configuration)
        {
            _container = container;
        }

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return Invoke(() => base.OnConnected(request, connectionId));
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Invoke(() => base.OnReceived(request, connectionId, data));
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            return Invoke(() => base.OnDisconnected(request, connectionId, stopCalled));
        }

        protected override Task OnReconnected(IRequest request, string connectionId)
        {
            return Invoke(() => base.OnReconnected(request, connectionId));
        }

        private async Task Invoke(Func<Task> method)
        {
            using (_container.BeginExecutionContextScope())
                await method();
        }
    }
}