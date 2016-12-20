using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Flexylexy.Web.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;

namespace Flexylexy.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();

            container.Options.EnableDynamicAssemblyCompilation = false;
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            var config = new HubConfiguration();

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new SimpleInjectorHubActivator(container));
            GlobalHost.DependencyResolver.Register(typeof(DiHubDispatcher), () => new DiHubDispatcher(config, container));
            //GlobalHost.HubPipeline.RequireAuthentication(); //if required otherwise comment out
            


            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            InjectionRegistrar.Register(container);

            //var resolver = new SignalRDependencyResolver(container);

            //container.Verify();


            //GlobalHost.DependencyResolver = resolver;


            //var activator = new SimpleInjectorHubActivator(container);
            //GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => activator);


            //container.AddRegistration(typeof(IHubConnectionContext<dynamic>), new );

            //    kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
            //resolver.Resolve<IConnectionManager>().GetHubContext<StockTickerHub>().Clients
            //    ).WhenInjectedInto<IStockTicker>();

            //app.MapSignalR(new HubConfiguration { Resolver = resolver });

            app.MapSignalR<DiHubDispatcher>("/signalr", config);
        }
    }
}