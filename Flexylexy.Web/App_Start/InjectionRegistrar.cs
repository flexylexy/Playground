using Flexylexy.Web.Models;
using SimpleInjector;

namespace Flexylexy.Web
{
    public class InjectionRegistrar
    {
        public static void Register(Container container)
        {
            container.Register<IRoster, Roster>(Lifestyle.Singleton);
            container.Register<ITokenizer, Tokenizer>(Lifestyle.Scoped);
            container.Register<IClient, Client>(Lifestyle.Scoped);
        }
    }
}