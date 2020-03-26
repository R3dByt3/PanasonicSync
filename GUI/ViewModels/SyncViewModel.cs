using Caliburn.Micro;
using DataStoring.Contracts.MovieModels;
using FFmpegStandardWrapper.Abstract.Core;
using FFmpegStandardWrapper.Abstract.Options;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.Messaging.Impl;
using PanasonicSync.GUI.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.ViewModels
{
    public class SyncViewModel : ViewModelBase, IScreen
    {
        private readonly ILogger _logger;
        private readonly IFFprobe _ffprobe;
        private readonly IFFmpeg _ffmpeg;
        private readonly IConversionOptions _conversionOptions;

        private IObservableCollection<IMovieFile> _movies;

        private List<Task> _downloadTasks;
        private List<Task> _conversionTasks;
        private List<Task> _transferTasks;

        private TaskFactory _downloadFactory;
        private TaskFactory _conversionFactory;
        private TaskFactory _transferFactory;

        private CancellationTokenSource _cts;

        public IObservableCollection<IMovieFile> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                NotifyOfPropertyChange();
            }
        }

        public SyncViewModel(IEnumerable<IMovieFile> movies)
        {
            _logger = _standardKernel.Get<ILoggerFactory>().CreateFileLogger();
            Movies = new BindableCollection<IMovieFile>(movies);

            //IList<Tuple<string, bool>> steps = new List<Tuple<string, bool>>();

            //foreach(var movie in Movies)
            //{
            //    steps.Add(movie.)
            //}

            //SendMessage(new SetProgressControlMessage(new ProgressbarViewModel
            //{
            //}));

            var downloadScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            var conversionScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            var transferScheduler = new LimitedConcurrencyLevelTaskScheduler(1);

            _downloadTasks = new List<Task>();
            _conversionTasks = new List<Task>();
            _transferTasks = new List<Task>();

            _downloadFactory = new TaskFactory(downloadScheduler);
            _conversionFactory = new TaskFactory(conversionScheduler);
            _transferFactory = new TaskFactory(transferScheduler);

            Directory.CreateDirectory("tmp");

            _ffmpeg = _standardKernel.Get<IFFmpeg>();
            _ffprobe = _standardKernel.Get<IFFprobe>();
            _conversionOptions = _standardKernel.Get<IConversionOptions>();
            _conversionOptions.VideoOptions.VideoBitrate = 4 * 1024 * 1024;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                _cts = new CancellationTokenSource();

                foreach (var movie in Movies.Where(x => x.IsSelected))
                {
                    Task task = _downloadFactory.StartNew(() => Download(movie), _cts.Token);
                    _downloadTasks.Add(task);
                }

                Task.WaitAll(_downloadTasks.ToArray());
                Task.WaitAll(_conversionTasks.ToArray());
                Task.WaitAll(_transferTasks.ToArray());

                _cts.Dispose();
            });
        }

        private void Download(IMovieFile movie)
        {
            try
            {
                movie.FilePath = $@"tmp\{movie.Title}.tts";
                _logger.Debug($"Download started for [{movie.FileLink}]-[{movie.FilePath}]");
                using (var client = new WebClient())
                {
                    client.DownloadFile(movie.FileLink, movie.FilePath);
                }
                _logger.Debug($"Download finished for [{movie.FileLink}]-[{movie.FilePath}]");

                Task task = _conversionFactory.StartNew(() => Convert(movie), _cts.Token);
                _conversionTasks.Add(task);
            }
            catch (Exception ex)
            {
                _logger.Error($"Download failed for [{movie.FileLink}]-[{movie.FilePath}]", ex);
            }
        }

        private void Convert(IMovieFile movie)
        {
            var newPath = Path.ChangeExtension(movie.FilePath, ".mp4");
            try
            {
                _logger.Debug($"Conversion started for [{movie.FilePath}]-[{newPath}]");
                _ffmpeg.Convert(movie.FilePath, newPath, _conversionOptions);
                _logger.Debug($"Conversion finished for [{movie.FilePath}]-[{newPath}]");
                File.Delete(movie.FilePath);
                _logger.Debug($"Deleted [{movie.FilePath}]");
                movie.FilePath = newPath;

                Task task = _transferFactory.StartNew(() => Transfer(movie), _cts.Token);
                _transferTasks.Add(task);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Conversion / Deletion failed for [{movie.FilePath}]-[{newPath}]");
            }
        }

        private void Transfer(IMovieFile movie)
        {
            var fileName = Path.GetFileName(movie.FilePath);
            var targetPath = Path.Combine(@"N:\Movies\ALLE_RECS", fileName);
            try
            {
                _logger.Debug($"Move started for [{movie.FilePath}]-[{targetPath}]");
                File.Move(movie.FilePath, targetPath);
                _logger.Debug($"Move finished for [{movie.FilePath}]-[{targetPath}]");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Move failed for [{movie.FilePath}]-[{targetPath}]");
            }
        }
    }
}
