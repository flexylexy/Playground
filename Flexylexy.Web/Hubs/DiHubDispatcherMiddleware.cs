using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using SimpleInjector;

namespace Flexylexy.Web.Hubs
{
    public class DiHubDispatcherMiddleware : OwinMiddleware
    {
        private readonly HubConfiguration _configuration;
        private readonly Container _container;
        private static readonly Task _emptyTask = MakeEmpty();

        public DiHubDispatcherMiddleware(OwinMiddleware next, HubConfiguration configuration, Container container)
          : base(next)
      {
            _configuration = configuration;
            _container = container;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(paramName: "context");
            }

            if (TryRejectJSONPRequest(_configuration, context))
            {
                return _emptyTask;
            }

            //var dispatcher = new HubDispatcher(_configuration);
            var dispatcher = new DiHubDispatcher(_configuration, _container);

            dispatcher.Initialize(_configuration.Resolver);

            return dispatcher.ProcessRequest(context.Environment);
        }

        internal static bool TryRejectJSONPRequest(ConnectionConfiguration config, IOwinContext context)
        {
            // If JSONP is enabled then do nothing
            if (config.EnableJSONP)
            {
                return false;
            }

            string callback = context.Request.Query.Get(key: "callback");

            // The request isn't a JSONP request so do nothing
            if (string.IsNullOrEmpty(callback))
            {
                return false;
            }

            // Disable the JSONP request with a 403
            context.Response.StatusCode = 403;
            context.Response.ReasonPhrase = "Forbidden JSONPDisabled";
            return true;
        }

        private static Task MakeEmpty()
        {
            return FromResult<object>(null);
        }

        public static Task<T> FromResult<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
    }
}