using FFmpegStandardWrapper.Abstract.Options;
using System.Text;

namespace FFmpegStandardWrapper.Options
{
    public class ConversionOptions : IConversionOptions
    {
        public IVideoOptions VideoOptions { get; set; }
        public IAudioOptions AudioOptions { get; set; }
        public ISubTitleOptions SubTitleOptions { get; set; }

        public ConversionOptions(IVideoOptions videoOptions, IAudioOptions audioOptions, ISubTitleOptions subTitleOptions)
        {
            VideoOptions = videoOptions;
            AudioOptions = audioOptions;
            SubTitleOptions = subTitleOptions;
        }

        public ConversionOptions()
        {

        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (VideoOptions != default(IVideoOptions))
                builder.Append(VideoOptions);
            if (AudioOptions != default(IAudioOptions))
                builder.Append(AudioOptions);
            if (SubTitleOptions != default(ISubTitleOptions))
                builder.Append(SubTitleOptions);

            return builder.ToString();
        }
    }
}
