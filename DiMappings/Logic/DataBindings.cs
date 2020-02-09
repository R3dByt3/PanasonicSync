using DataStoring;
using DataStoring.Contracts;
using DataStoring.Contracts.PanasonicRequest;
using DataStoring.Contracts.PanasonicResponse;
using DataStoring.Contracts.UpnpResponse;
using DataStoring.PanasonicRequest;
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

            Bind<IStartingIndex>().To<StartingIndex>();
            Bind<ISortCriteria>().To<SortCriteria>();
            Bind<IRequestedCount>().To<RequestedCount>();
            Bind<IObjectID>().To<ObjectID>();
            Bind<IFilter>().To<Filter>();
            Bind<IBrowseFlag>().To<BrowseFlag>();
            Bind<IBrowse>().To<Browse>();
            Bind<IRequestBody>().To<RequestBody>();
            Bind<IEnvelope>().To<Envelope>();

            Bind<IPanasonicDevice>().To<PanasonicDevice>();

            Bind<IChannelID>().To<ChannelID>();
            Bind<IClass>().To<Class>();
            Bind<IRemoteMovie>().To<RemoteMovie>();
            Bind<IItem>().To<Item>();
            Bind<IMovieListResponse>().To<MovieListResponse>();
            Bind<IBrowseResponse>().To<BrowseResponse>();
            Bind<IResponseBody>().To<ResponseBody>();
        }
    }
}
