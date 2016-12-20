using Flexylexy.Web.Hubs;
using Flexylexy.Web.Models;
using SimpleInjector;

namespace Flexylexy.Web
{
    public class InjectionRegistrar
    {
        public static void Register(Container container)
        {
            container.Register<IRoster, Roster>(Lifestyle.Singleton);
            container.Register<ITokenizer, Tokenizer>(Lifestyle.Singleton);
            container.Register<IClient, Client>(Lifestyle.Scoped);

            container.Register(() =>
            {
                var roster = (IRoster)container.GetInstance(typeof (IRoster));
                var tokenizer = (ITokenizer)container.GetInstance(typeof (ITokenizer));
                return new GamesHub(roster, tokenizer);
            }, Lifestyle.Scoped);

            container.Register(() =>
            {
                var roster = (IRoster)container.GetInstance(typeof(IRoster));
                var tokenizer = (ITokenizer)container.GetInstance(typeof(ITokenizer));
                return new ChatHub(roster, tokenizer);
            }, Lifestyle.Scoped);
        }
    }
}