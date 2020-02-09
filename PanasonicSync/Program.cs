using APIClient.Contracts.Panasonic;
using DiMappings;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpnpClient.Contracts;

namespace PanasonicSync
{
    class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new Aggregator().Mappings);

            var client = kernel.Get<IClient>();
            var panaclient = kernel.Get<IPanasonicClient>();

            var device = client.SearchUpnpDevices().First();

            panaclient.LoadControlsUri(device);
            var list = panaclient.RequestMovies().ToList();

            Console.ReadLine();
        }
    }
}
