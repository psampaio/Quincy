using System.Linq;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace Quincy.Commands
{
    public class GenerateCommandConvention : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            var commandGenerators = types
                .FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
                .Where(t => typeof(IGenerateCommand).IsAssignableFrom(t));
            foreach (var type in commandGenerators)
                registry.For(type).Use(type);
        }
    }
}