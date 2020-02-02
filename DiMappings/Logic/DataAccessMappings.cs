using DataStoring;
using DataStoring.Contracts;
using Ninject.Modules;

namespace DiMappings.Logic
{
    class DataAccessMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataAccess>().To<DataAccess>();
        }
    }
}
