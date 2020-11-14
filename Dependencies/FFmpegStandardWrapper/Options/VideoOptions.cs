using FFmpegStandardWrapper.Abstract.Model;
using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.Exceptions;
using System.Text;

namespace FFmpegStandardWrapper.Options
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class VideoOptions : IVideoOptions
    {
        private ICodec _codec;

        public ulong OutputFrames { get; set; }
        public double FrameRate { get; set; }
        public ISize Size { get; set; }
        public IAspect Aspect { get; set; }
        public bool DisableVideo { get; set; }
        public ICodec Codec
        {
            get => _codec;
            set
            {
                if (!value.IsVideoCodec)
                    throw new InvalidCodecException("The selected codec is no video codec");
                _codec = value;
            } 
        }
        public ulong VideoBitrate { get; set; }
        public ulong AudioBitrate { get; set; }

        public VideoOptions(ISize size, IAspect aspect)
        {
            Size = size;
            Aspect = aspect;
        }

        public VideoOptions()
        {

        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (OutputFrames != 0)
                builder.Append($"-vframes {OutputFrames} ");
            if (FrameRate != 0)
                builder.Append($"-r {FrameRate} ");
            if (Size.HasValue)
                builder.Append($"-s {Size} ");
            if (Aspect.HasValue)
                builder.Append($"-aspect {Aspect} ");
            if (DisableVideo)
                builder.Append($"-vn ");
            if (Codec != default(ICodec))
                builder.Append($"-vcodec {Codec} ");
            if (VideoBitrate != 0)
                builder.Append($"-b:v {VideoBitrate} ");
            if (AudioBitrate != 0)
                builder.Append($"-b:a {AudioBitrate} ");

            return builder.ToString();
        }
    }
}
