using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using SimpleInjector;

namespace Flexylexy.Web
{
    public class SimpleInjectorHubActivator : IHubActivator
    {
        private readonly Container _container;

        public SimpleInjectorHubActivator(Container container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub)_container.GetInstance(descriptor.HubType);
        }
    }
}