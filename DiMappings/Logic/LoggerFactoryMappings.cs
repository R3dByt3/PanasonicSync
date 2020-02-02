using NetStandard.Logger;
using Ninject.Modules;

namespace DiMappings.Logic
{
    internal class LoggerFactoryMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILoggerFactory>().To<LoggerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("Renter")
                .WithConstructorArgument(LoggingLevel.Debug);
        }
    }
}
