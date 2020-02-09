using DataStoring;
using DataStoring.Contracts;
using DataStoring.Contracts.UpnpResponse;
using DataStoring.PanasonicResponse;
using DataStoring.UpnpResponse;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiMappings.Logic
{
    public class DataBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ISettings>().To<Settings>();

            Bind<IPanasonicDevice>().To<PanasonicDevice>();
        }
    }
}
