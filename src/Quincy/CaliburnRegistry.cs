using Caliburn.Micro;
using StructureMap;

namespace Quincy
{
    public class CaliburnRegistry : Registry
    {
        public CaliburnRegistry()
        {
            Policies.Interceptors(new EventAggregatorHandlerRegistration());

            For<IWindowManager>().Use<WindowManager>().Singleton();
            For<IEventAggregator>().Use<EventAggregator>().Singleton();
        }
    }
}