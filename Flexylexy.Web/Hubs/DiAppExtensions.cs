using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.AspNet.SignalR.Tracing;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;

namespace Flexylexy.Web.Hubs
{
    public static class DiAppExtensions
    {
        public static IAppBuilder MapMyDispatch(this IAppBuilder builder, HubConfiguration configuration, Container container)
        {
            return builder.Map(pathMatch: "/signalr", configuration: subApp => subApp.RunSignalR(configuration, container));
        }
        public static void RunSignalR(this IAppBuilder builder, HubConfiguration configuration, Container container)
        {
            builder.UseMyMiddleware<DiHubDispatcherMiddleware>(new object[] { configuration, container });// the order of the arguments is important
        }
        private static IAppBuilder UseMyMiddleware<T>(this IAppBuilder builder, params object[] args)
        {
            EnsureValidCulture();
            ConnectionConfiguration configuration = null;

            // Ensure we have the conversions for MS.Owin so that
            // the app builder respects the OwinMiddleware base class
            SignatureConversions.AddConversions(builder);

            if (args.Length > 0)
            {
                configuration = args[args.Length - 2] as ConnectionConfiguration; // the order of the arguments is important

                if (configuration == null)
                {
                    throw new ArgumentException(message: "NoConfiguration");
                }

                var resolver = configuration.Resolver;

                if (resolver == null)
                {
                    throw new ArgumentException(message: "NoDependencyResolver");
                }

                var env = builder.Properties;
                CancellationToken token = env.GetShutdownToken();

                // If we don't get a valid instance name then generate a random one
                string instanceName = env.GetAppInstanceName() ?? Guid.NewGuid().ToString();

                // Use the data protection provider from app builder and fallback to the
                // Dpapi provider
                IDataProtectionProvider provider = builder.GetDataProtectionProvider();
                IProtectedData protectedData;

                // If we're using DPAPI then fallback to the default protected data if running
                // on mono since it doesn't support any of this
                //if (provider == null && MonoUtility.IsRunningMono)
                //{
                //   protectedData = new DefaultProtectedData();
                //}
                //else
                //{
                if (provider == null)
                {
                    provider = new DpapiDataProtectionProvider(instanceName);
                }

                protectedData = new DataProtectionProviderProtectedData(provider);
                //}

                resolver.Register(typeof(IProtectedData), () => protectedData);

                // If the host provides trace output then add a default trace listener
                TextWriter traceOutput = env.GetTraceOutput();
                if (traceOutput != null)
                {
                    var hostTraceListener = new TextWriterTraceListener(traceOutput);
                    var traceManager = new TraceManager(hostTraceListener);
                    resolver.Register(typeof(ITraceManager), () => traceManager);
                }

                // Try to get the list of reference assemblies from the host
                IEnumerable<Assembly> referenceAssemblies = env.GetReferenceAssemblies();
                if (referenceAssemblies != null)
                {
                    // Use this list as the assembly locator
                    var assemblyLocator = new EnumerableOfAssemblyLocator(referenceAssemblies);
                    resolver.Register(typeof(IAssemblyLocator), () => assemblyLocator);
                }

                resolver.InitializeHost(instanceName, token);
            }

            // The order of the arguments is important due to commented out 
            //      if (!parameterTypes
            //          .Skip(1)
            //          .Zip(args, TestArgForParameter)
            //          .All(x => x))
            //      {
            //         continue;
            //      }
            // in ToConstructorMiddlewareFactory from ToMiddlewareFactory in https://github.com/owin/museum-piece-owin-hosting/blob/master/src/main/Owin.Builder/AppBuilder.cs
            // see below end of this function
            builder.Use(typeof(T), args);

            // BUG 2306: We need to make that SignalR runs before any handlers are
            // mapped in the IIS pipeline so that we avoid side effects like
            // session being enabled. The session behavior can be
            // manually overridden if user calls SetSessionStateBehavior but that shouldn't
            // be a problem most of the time.
            builder.UseStageMarker(PipelineStage.PostAuthorize);

            return builder;
        }

        //// Type Constructor pattern: public Delta(AppFunc app, string arg1, string arg2)
        //private static Tuple<Type, Delegate, object[]> ToConstructorMiddlewareFactory(object middlewareObject, object[] args, ref Delegate middlewareDelegate)
        //{
        //   Type middlewareType = middlewareObject as Type;
        //   ConstructorInfo[] constructors = middlewareType.GetConstructors();
        //   foreach (ConstructorInfo constructor in constructors)
        //   {
        //      ParameterInfo[] parameters = constructor.GetParameters();
        //      Type[] parameterTypes = parameters.Select(p => p.ParameterType).ToArray();
        //      if (parameterTypes.Length != args.Length + 1)
        //      {
        //         continue;
        //      }
        //      if (!parameterTypes
        //          .Skip(1)
        //          .Zip(args, TestArgForParameter)
        //          .All(x => x))
        //      {
        //         continue;
        //      }

        //      ParameterExpression[] parameterExpressions = parameters.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
        //      NewExpression callConstructor = Expression.New(constructor, parameterExpressions);
        //      middlewareDelegate = Expression.Lambda(callConstructor, parameterExpressions).Compile();
        //      return Tuple.Create(parameters[0].ParameterType, middlewareDelegate, args);
        //   }

        //   throw new MissingMethodException(middlewareType.FullName,
        //       string.Format(CultureInfo.CurrentCulture, "BuilderResources.Exception_NoConstructorFound", args.Length + 1));
        //}
        //private static bool TestArgForParameter(Type parameterType, object arg)
        //{
        //   return (arg == null && !parameterType.IsValueType) ||
        //       parameterType.IsInstanceOfType(arg);
        //}
        private static void EnsureValidCulture()
        {
            // The CultureInfo may leak across app domains which may cause hangs. The most prominent
            // case in SignalR are MapSignalR hangs when creating Performance Counters (#3414).
            // See https://github.com/SignalR/SignalR/issues/3414#issuecomment-152733194 for more details.
            var culture = CultureInfo.CurrentCulture;
            while (!culture.Equals(CultureInfo.InvariantCulture))
            {
                culture = culture.Parent;
            }

            if (ReferenceEquals(culture, CultureInfo.InvariantCulture))
            {
                return;
            }

            var thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.GetCultureInfo(thread.CurrentCulture.Name);
            thread.CurrentUICulture = CultureInfo.GetCultureInfo(thread.CurrentUICulture.Name);
        }

        internal static CancellationToken GetShutdownToken(this IDictionary<string, object> env)
        {
            object value;
            return env.TryGetValue(key: "host.OnAppDisposing", value: out value)
                && value is CancellationToken
                ? (CancellationToken)value
                : default(CancellationToken);
        }
        internal static string GetAppInstanceName(this IDictionary<string, object> environment)
        {
            object value;
            if (environment.TryGetValue(key: "host.AppName", value: out value))
            {
                var stringVal = value as string;

                if (!string.IsNullOrEmpty(stringVal))
                {
                    return stringVal;
                }
            }

            return null;
        }
        internal static TextWriter GetTraceOutput(this IDictionary<string, object> environment)
        {
            object value;
            if (environment.TryGetValue(key: "host.TraceOutput", value: out value))
            {
                return value as TextWriter;
            }

            return null;
        }
        internal static IEnumerable<Assembly> GetReferenceAssemblies(this IDictionary<string, object> environment)
        {
            object assembliesValue;
            if (environment.TryGetValue(key: "host.ReferencedAssemblies", value: out assembliesValue))
            {
                return (IEnumerable<Assembly>)assembliesValue;
            }

            return null;
        }
    }
}