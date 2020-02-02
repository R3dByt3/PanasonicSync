using NetStandard.IO.Compression;
using Ninject.Modules;

namespace DiMappings.Logic
{
    internal class CompressorMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<ICompressor>().To<Compressor>();
        }
    }
}
