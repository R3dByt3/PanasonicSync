using FFmpegStandardWrapper.Abstract.Core;
using FFmpegStandardWrapper.Exceptions;
using FFmpegStandardWrapper.Model.Probe;
using NetStandard.Logger;
using Newtonsoft.Json;
using System;

namespace FFmpegStandardWrapper.Core
{
    public class FFprobe : ProcessorBase, IFFprobe
    {
        private readonly ILogger _logger;
        private readonly string _ffprobePath;

        public FFprobe(ILoggerFactory factory)
        {
            _logger = factory.CreateFileLogger();
            if (string.IsNullOrWhiteSpace(Controller.FFProbePath))
                throw new FFmpegException("Engine has not been initialzied yet");

            _ffprobePath = Controller.FFProbePath;
        }

        public DetailedMovieInformation GetDetailedMovieInformation(string videoPath)
        {
            var json = RunFFprobe($"-v quiet -print_format json -sexagesimal -show_streams -show_format \"{videoPath}\"");
            var details = JsonConvert.DeserializeObject<DetailedMovieInformation>(json);
            return details;
        }

        public TimeSpan GetVideoLenght(string videoPath)
        {
            return GetDetailedMovieInformation(videoPath).Format.Duration;
        }
    }
}
