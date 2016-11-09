using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;

namespace Flexylexy.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //private static Container _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //InitializeInjection();
        }

        //private void InitializeInjection()
        //{
        //    _container = new Container();
        //    _container.Options.EnableDynamicAssemblyCompilation = false;
        //    _container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
        //    _container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
        //    DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_container));

        //    InjectionRegistrar.Register(_container);

        //    _container.;

        //    var resolver = new SignalRDependencyResolver(_container);

        //    kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
        //resolver.Resolve<IConnectionManager>().GetHubContext<StockTickerHub>().Clients
        //    ).WhenInjectedInto<IStockTicker>();

        //    var config = new HubConfiguration();
        //    config.Resolver = resolver;
        //    Microsoft.AspNet.SignalR.StockTicker.Startup.ConfigureSignalR(app, config);

        //    _container.Verify();
        //}
    }
}