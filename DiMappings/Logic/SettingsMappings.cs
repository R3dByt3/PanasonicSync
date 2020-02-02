using DataStoring;
using DataStoring.Contracts;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class SettingsMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>();
        }
    }
}
