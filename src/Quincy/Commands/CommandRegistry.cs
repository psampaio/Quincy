using System.Collections.Generic;
using System.Reflection;
using StructureMap;

namespace Quincy.Commands
{
    public class CommandRegistry : Registry
    {
        public CommandRegistry(IEnumerable<Assembly> assemblies)
        {
            Scan(s =>
            {
                foreach (var assembly in assemblies)
                    s.Assembly(assembly);

                s.Convention<GenerateCommandConvention>();
            });

            For<ICommandManager>().Use<CommandManager>().Singleton();
            For<CommandGeneratorFactory>().Use<CommandGeneratorFactory>(ctx => t => (IGenerateCommand) ctx.GetInstance(t));
        }
    }
}