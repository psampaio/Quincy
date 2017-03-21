using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using StructureMap;

namespace Quincy
{
    public abstract class StructureMapBootstrapper : BootstrapperBase
    {
        private IContainer container;

        protected StructureMapBootstrapper(bool useApplication = true)
            : base(useApplication)
        {
        }

        protected override void Configure()
        {
            container = ConfigureContainer();
        }

        protected abstract IContainer ConfigureContainer();

        protected override object GetInstance(Type serviceType, string key)
        {
            return string.IsNullOrEmpty(key)
                ? container.GetInstance(serviceType)
                : container.GetInstance(serviceType, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetAllInstances(serviceType).OfType<object>();
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}