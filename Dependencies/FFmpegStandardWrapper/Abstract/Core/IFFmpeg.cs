using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.EventArgs;
using FFmpegStandardWrapper.Model.Probe;
using System;

namespace FFmpegStandardWrapper.Abstract.Core
{
    public interface IFFmpeg
    {
        void Convert(string inputFile, string outputFile, IConversionOptions options);
        event EventHandler<ConversionCompleteEventArgs> ConversionCompleteEvent;
        event EventHandler<ConvertProgressEventArgs> ConvertProgressEvent;
    }
}
