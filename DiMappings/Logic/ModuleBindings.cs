using APIClient.Contracts.Panasonic;
using APIClient.Panasonic;
using Configuration;
using Configuration.Contracts;
using DataStorage;
using DataStorage.Contracts;
using DataStoring;
using DataStoring.Contracts;
using NetStandard.IO.Compression;
using NetStandard.IO.Files;
using NetStandard.Logger;
using Ninject.Modules;
using UpnpClient;
using UpnpClient.Contracts;

namespace DiMappings.Logic
{
    internal class ModuleBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoggerFactory>().To<LoggerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("PanasonicSync")
                .WithConstructorArgument(LoggingLevel.Debug);
            Bind<IFileManager>().To<FileManager>();
            Bind<ICompressor>().To<Compressor>();
            Bind<IConfigurator>().To<Configurator>();
            Bind<IDatabaseAccess>().To<DatabaseAccess>();
            Bind<IClient>().To<Client>();
            Bind<IPanasonicClient>().To<PanasonicClient>();
        }
    }
}
