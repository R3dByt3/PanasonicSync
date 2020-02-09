using DiMappings;
using Ninject;

namespace PanasonicSync.GUI
{
    public static class Controller
    {
        public static IKernel Kernel = new StandardKernel(new Aggregator().Mappings);
    }
}
