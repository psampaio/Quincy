using System.Collections.Generic;
using System.Reflection;
using Caliburn.Micro;
using NSubstitute;
using StructureMap;
using Xunit;

namespace Quincy.Tests
{
    public class BootstrapperTests
    {
        public BootstrapperTests()
        {
            container = Substitute.For<IContainer>();
            var testBootstrapper = new TestBootstrapper(container);
            testBootstrapper.Initialize();
        }

        private readonly IContainer container;

        [Fact]
        public void BuildUpServices()
        {
            var objectToBuild = new ConcreteFoo();
            IoC.BuildUp(objectToBuild);
            container.Received(1).BuildUp(Arg.Is(objectToBuild));
        }

        [Fact]
        public void GetAllServices()
        {
            IoC.GetAll<IFoo>();
            container.Received(1).GetAllInstances(Arg.Is(typeof(IFoo)));
        }

        [Fact]
        public void GetASingleServiceWithKey()
        {
            var serviceKey = "MyService";
            IoC.Get<IFoo>(serviceKey);
            container.Received(1).GetInstance(Arg.Is(typeof(IFoo)), serviceKey);
        }

        [Fact]
        public void GetASingleServiceWithoutKey()
        {
            IoC.Get<IFoo>();
            container.Received(1).GetInstance(Arg.Is(typeof(IFoo)));
        }
    }

    public interface IFoo
    {
    }

    public class ConcreteFoo
    {
    }

    public class TestBootstrapper : Bootstrapper
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

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
            {
                GetType().Assembly
            };
        }
    }
}