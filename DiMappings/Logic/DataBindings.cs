using DataStoring;
using DataStoring.Contracts;
using DataStoring.Contracts.MovieModels;
using DataStoring.Contracts.UpnpResponse;
using DataStoring.MovieModels;
using DataStoring.UpnpResponse;
using Ninject.Modules;

namespace DiMappings.Logic
{
    public class DataBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>()
                .InSingletonScope();

            Bind<IPanasonicDevice>().To<PanasonicDevice>();

            Bind<IMovieFile>().To<MovieFile>();
            Bind<IConflict>().To<Conflict>();
        }
    }
}
