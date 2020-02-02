using Configuration;
using Configuration.Contracts;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class ConfiguratorMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigurator>().To<Configurator>();
        }
    }
}
