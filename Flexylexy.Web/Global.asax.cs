using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;

namespace Flexylexy.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Container _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeInjection();
        }

        private void InitializeInjection()
        {
            _container = new Container();
            _container.Options.EnableDynamicAssemblyCompilation = false;
            _container.Options.DefaultScopedLifestyle = (ScopedLifestyle)Lifestyle.Transient;


            InjectionRegistrar.Register(_container);
        }
    }
}