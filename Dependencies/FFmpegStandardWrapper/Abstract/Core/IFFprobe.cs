using FFmpegStandardWrapper.Model.Probe;
using System;

namespace FFmpegStandardWrapper.Abstract.Core
{
    public interface IFFprobe
    {
        DetailedMovieInformation GetDetailedMovieInformation(string videoPath);
        TimeSpan GetVideoLenght(string videoPath);
    }
}
