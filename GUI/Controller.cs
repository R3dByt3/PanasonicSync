using DiMappings;
using Ninject;

namespace GUI
{
    public static class Controller
    {
        public static IKernel Kernel = new StandardKernel(new Aggregator().Mappings);
    }
}
