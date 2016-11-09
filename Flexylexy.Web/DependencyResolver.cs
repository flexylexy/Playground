using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using SimpleInjector;

namespace Flexylexy.Web
{
    public class SignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly Container _container;

        public SignalRDependencyResolver(Container container)
        {
            _container = container;
        }

        public override object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType) ?? base.GetServices(serviceType);
        }
    }
}