using System.Collections.Generic;
using System.Reflection;
using StructureMap;

namespace Quincy.Events
{
    public class EventRegistry : Registry
    {
        public EventRegistry(IEnumerable<Assembly> assemblies)
        {
            Scan(scanner =>
            {
                foreach (var assembly in assemblies)
                    scanner.Assembly(assembly);


                scanner.AddAllTypesOf<IStoreStateProvider>();
                scanner.ConnectImplementationsToTypesClosing(typeof(IEventHandler<,>));
            });

            For<IStore>().Use<Store>().Singleton();
        }
    }
}