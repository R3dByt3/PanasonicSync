using DiMappings.Logic;
using Ninject.Modules;

namespace DiMappings
{
    public class Aggregator
    {
#pragma warning disable CA1819 // Properties should not return arrays
        public INinjectModule[] Mappings => new INinjectModule[]
#pragma warning restore CA1819 // Properties should not return arrays
        {
            new DataBindings(),
            new ModuleBindings(),
        };
    }
}
