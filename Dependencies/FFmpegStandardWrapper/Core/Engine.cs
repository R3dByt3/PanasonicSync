using FFmpegStandardWrapper.Abstract.Core;
using FFmpegStandardWrapper.Abstract.Crawling;
using FFmpegStandardWrapper.Abstract.Model;
using FFmpegStandardWrapper.Crawling;
using FFmpegStandardWrapper.Model;
using NetStandard.IO.Compression;
using NetStandard.Logger;
using System;
using System.IO;

namespace FFmpegStandardWrapper.Core
{
    public class Engine : ProcessorBase, IEngine
    {
        private const string BinPath = "bin";

        private readonly ILogger _logger;
        private readonly IFFmpegCrawler _ffmpegCrawler;
        private readonly string _ffmpegDirectory;

        public Engine(ILoggerFactory factory, ICompressor compressor)
        {
            _logger = factory.CreateFileLogger();
            _ffmpegDirectory = Path.Combine(Directory.GetCurrentDirectory(), BinPath);
            _ffmpegCrawler = new FFmpegCrawler(factory, compressor, _ffmpegDirectory);

            Directory.CreateDirectory(_ffmpegDirectory);

            if (_ffmpegCrawler.CurrentFFmpegVersion == null & !_ffmpegCrawler.TryLoadLatestVersion())
                throw new FileNotFoundException("There is no ffmpeg version available or downloadable");

            Controller.FFmpegPath = Path.Combine(_ffmpegDirectory, "ffmpeg.exe");
            Controller.FFProbePath = Path.Combine(_ffmpegDirectory, "ffprobe.exe");

            LoadCodecs();
            LoadMuxers("demuxers");
            LoadMuxers("muxers");
            LoadFormats();

            //Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), BinPath));
        }

        private void LoadFormats()
        {
            var result = RunFFmpeg($"-v quiet -formats");

            foreach (var line in result.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                IFormat format = new Format();
                if (format.Fill(line))
                    Format.Formats.Add(format);
            }
        }

        private void LoadCodecs()
        {
            var result = RunFFmpeg($"-v quiet -codecs");

            foreach (var line in result.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                ICodec codec = new Codec();
                if (codec.Fill(line))
                    Codec.Codecs.Add(codec);
            }
        }

        private void LoadMuxers(string muxerType)
        {
            var result = RunFFmpeg($"-v quiet -{muxerType}");

            foreach (var line in result.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                IMuxer muxer = new Muxer();
                if (muxer.Fill(line))
                    Muxer.Muxers.Add(muxer);
            }
        }

        public void CleanUpFFmpeg()
        {
            Directory.Delete(_ffmpegDirectory, true);
        }
    }
}
