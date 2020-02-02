using ExtendedIO.MediaToolKitSet;
using Ninject.Modules;

namespace DiMappings.Logic
{
    internal class MediaManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMediaManager>().To<MediaManager>();
        }
    }
}
