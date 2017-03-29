using System.Collections.Generic;
using System.Reflection;
using MediatR;
using StructureMap;

namespace Quincy
{
    public class MediatorRegistry : Registry
    {
        public MediatorRegistry(IEnumerable<Assembly> assemblies)
        {
            Scan(scanner =>
            {
                foreach (var assembly in assemblies)
                    scanner.Assembly(assembly);

                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));

                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            });

            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>().Singleton();
        }
    }
}