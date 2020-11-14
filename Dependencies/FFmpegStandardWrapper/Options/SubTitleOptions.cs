using FFmpegStandardWrapper.Abstract.Model;
using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.Exceptions;
using System.Text;

namespace FFmpegStandardWrapper.Options
{
    public class SubTitleOptions : ISubTitleOptions
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        private ICodec _codec;

        public ISize Size { get; set; }
        public bool DisableSubTitles { get; set; }
        public ICodec Codec 
        { 
            get => _codec;
            set
            {
                if (!value.IsSubtitleCodec)
                    throw new InvalidCodecException("The selected codec is no subtitle codec");
                _codec = value;
            }
        }
        public ISize CanvasSize { get; set; }

        public SubTitleOptions(ISize size, ISize canvasSize)
        {
            Size = size;
            CanvasSize = canvasSize;
        }

        public SubTitleOptions()
        {

        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (Size.HasValue)
                builder.Append($"-s {Size} ");
            if (DisableSubTitles)
                builder.Append($"-sn ");
            if (Codec != default(ICodec))
                builder.Append($"-scodec {Codec} ");
            if (CanvasSize.HasValue)
                builder.Append($"-canvas_size {CanvasSize} ");

            return builder.ToString();
        }
    }
}
