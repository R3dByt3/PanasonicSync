using DataStorage;
using DataStorage.Contracts;
using Ninject.Modules;

namespace DiMappings.Logic
{
    internal class DBAccessMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseAccess>().To<DatabaseAccess>();
        }
    }
}
