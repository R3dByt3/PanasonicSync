using DataStoring;
using DataStoring.Contracts;
using DataStoring.Contracts.MovieModels;
using DataStoring.Contracts.UpnpResponse;
using DataStoring.MovieModels;
using DataStoring.UpnpResponse;
using FFmpegStandardWrapper.Abstract.Model;
using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.Model;
using FFmpegStandardWrapper.Options;
using Ninject.Modules;

namespace DiMappings.Logic
{
    public class DataBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IPanasonicDevice>().To<PanasonicDevice>();

            Bind<IMovieFile>().To<MovieFile>();
            Bind<IConflict>().To<Conflict>();

            Bind<ICodec>().To<Codec>();
            Bind<ISize>().To<Size>();
            Bind<IAspect>().To<Aspect>();
            Bind<IAudioOptions>().To<AudioOptions>();
            Bind<ISubTitleOptions>().To<SubTitleOptions>();
            Bind<IVideoOptions>().To<VideoOptions>();
            Bind<IConversionOptions>().To<ConversionOptions>();
        }
    }
}
