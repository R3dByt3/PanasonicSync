using DiMappings;
using Ninject;
using System.Threading;
using TranslationsCore;

namespace PanasonicSync.GUI
{
    public static class Controller
    {
        public static IKernel Kernel = new StandardKernel(new Aggregator().Mappings);
        public static TranslationProvider TranslationProvider = TranslationProvider.LoadCulture(Thread.CurrentThread.CurrentCulture.LCID);
    }
}
