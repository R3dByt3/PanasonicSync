using NetStandard.IO.Files;
using Ninject.Modules;

namespace DiMappings.Logic
{
    internal class FileManagerMappings : NinjectModule
    {
        public override void Load()
        {
            Bind<IFileManager>().To<FileManager>();
        }
    }
}
