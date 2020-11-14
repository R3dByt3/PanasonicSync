using FFmpegStandardWrapper.Abstract.Model;
using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.Exceptions;
using System.Text;

namespace FFmpegStandardWrapper.Options
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class AudioOptions : IAudioOptions
    {
        private ICodec _codec;

        public ulong OutputFrames { get; set; }
        public double SampleRate { get; set; }
        public bool DisableAudio { get; set; }
        public ICodec Codec 
        { 
            get => _codec;
            set
            {
                if (!value.IsAudioCodec)
                    throw new InvalidCodecException("The selected codec is no audio codec");
                _codec = value;
            }
        }

        //public AudioOptions(ICodec codec)
        //{
        //    _codec = codec;
        //}

        public AudioOptions()
        {

        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (OutputFrames != 0)
                builder.Append($"-aframes {OutputFrames} ");
            if (SampleRate != 0)
                builder.Append($"-ar {SampleRate} ");
            if (DisableAudio)
                builder.Append($"-an ");
            if (Codec != default(ICodec))
                builder.Append($"-acodec {Codec} ");

            return builder.ToString();
        }
    }
}
