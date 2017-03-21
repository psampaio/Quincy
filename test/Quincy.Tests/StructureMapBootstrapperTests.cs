using Caliburn.Micro;
using NSubstitute;
using StructureMap;
using Xunit;

namespace Quincy.Tests
{
    public class StructureMapBootstrapperTests
    {
        private readonly IContainer container;

        public StructureMapBootstrapperTests()
        {
            container = Substitute.For<IContainer>();
            var testBootstrapper = new TestBootstrapper(container);
            testBootstrapper.Initialize();
        }

        [Fact]
        public void BuildUpServices()
        {
            var objectToBuild = new ConcreteFoo();
            IoC.BuildUp(objectToBuild);
            container.Received(1).BuildUp(Arg.Is(objectToBuild));
        }

        [Fact]
        public void GetASingleServiceWithoutKey()
        {
            IoC.Get<IFoo>();
            container.Received(1).GetInstance(Arg.Is(typeof(IFoo)));
        }

        [Fact]
        public void GetASingleServiceWithKey()
        {
            var serviceKey = "MyService";
            IoC.Get<IFoo>(serviceKey);
            container.Received(1).GetInstance(Arg.Is(typeof(IFoo)), serviceKey);
        }

        [Fact]
        public void GetAllServices()
        {
            IoC.GetAll<IFoo>();
            container.Received(1).GetAllInstances(Arg.Is(typeof(IFoo)));
        }
    }

    public interface IFoo
    {
    }

    public class ConcreteFoo
    {
        
    }

    public class TestBootstrapper : StructureMapBootstrapper
    {
        private readonly IContainer containerToConfigure;

        public TestBootstrapper(IContainer container) :
            base(false)
        {
            containerToConfigure = container;
        }

        protected override IContainer ConfigureContainer()
        {
            return containerToConfigure;
        }
    }

}
