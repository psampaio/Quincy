using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using StructureMap.Building.Interception;
using StructureMap.Pipeline;
using StructureMap.TypeRules;

namespace Quincy
{
    public class EventAggregatorHandlerRegistration : IInterceptorPolicy
    {
        public string Description => "Adds the constructed object to the EventAggregator";

        public IEnumerable<IInterceptor> DetermineInterceptors(Type pluginType, Instance instance)
        {
            if (!instance.ReturnedType.FindInterfacesThatClose(typeof(IHandle<>)).Any()) yield break;
            yield return new ActivatorInterceptor<IHandle>((c, h) => c.GetInstance<IEventAggregator>().Subscribe(h));
        }
    }
}