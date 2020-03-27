using Caliburn.Micro;
using DataStoring.Contracts;
using DataStoring.Contracts.MovieModels;
using FFmpegStandardWrapper.Abstract.Core;
using FFmpegStandardWrapper.Abstract.Options;
using FFmpegStandardWrapper.EventArgs;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.Enums;
using PanasonicSync.GUI.Extensions;
using PanasonicSync.GUI.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly ISettings _settings;

        private readonly List<Task> _downloadTasks;
        private readonly List<Task> _conversionTasks;
        private readonly List<Task> _transferTasks;

        private readonly TaskFactory _downloadFactory;
        private readonly TaskFactory _conversionFactory;
        private readonly TaskFactory _transferFactory;
        private CancellationTokenSource _cts;

        private IObservableCollection<IMovieFile> _movies;

        private ProgressbarViewModel _downloadProgressbar;
        private ProgressbarViewModel _conversionProgressbar;
        private ProgressbarViewModel _transferProgressbar;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange();
            }
        }

        public ProgressbarViewModel DownloadProgressbar
        {
            get => _downloadProgressbar;
            set
            {
                _downloadProgressbar = value;
                NotifyOfPropertyChange();
            }
        }
        public ProgressbarViewModel ConversionProgressbar
        {
            get => _conversionProgressbar;
            set
            {
                _conversionProgressbar = value;
                NotifyOfPropertyChange();
            }
        }
        public ProgressbarViewModel TransferProgressbar
        {
            get => _transferProgressbar;
            set
            {
                _transferProgressbar = value;
                NotifyOfPropertyChange();
            }
        }

        public IObservableCollection<IMovieFile> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanStop
        {
            get => _cts != null;
        }

        public SyncViewModel(IEnumerable<IMovieFile> movies)
        {
            Movies = new BindableCollection<IMovieFile>(movies);
            IsEnabled = true;

            _logger = _standardKernel.Get<ILoggerFactory>().CreateFileLogger();
            _settings = _standardKernel.Get<ISettings>();
            _ffmpeg = _standardKernel.Get<IFFmpeg>();
            _ffprobe = _standardKernel.Get<IFFprobe>();
            _conversionOptions = _standardKernel.Get<IConversionOptions>();

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
        }

        protected override void OnViewLoaded(object view)
        {
            DownloadProgressbar = new ProgressbarViewModel(false);
            ConversionProgressbar = new ProgressbarViewModel(false);
            TransferProgressbar = new ProgressbarViewModel(false);

            DownloadProgressbar.Maximum = 100;
            DownloadProgressbar.CurrentStep = TranslationProvider.Downloading;
            ConversionProgressbar.Maximum = 100;
            ConversionProgressbar.CurrentStep = TranslationProvider.Converting;
            TransferProgressbar.Maximum = 100;
            TransferProgressbar.CurrentStep = TranslationProvider.Transferring;

            //_conversionOptions.VideoOptions.VideoBitrate = 4 * 1024 * 1024;

            _ffmpeg.ConvertProgressEvent += ConversionProgress;
            //_ffmpeg.ConversionCompleteEvent += ConversionCompleted;
            base.OnViewLoaded(view);
        }

        private void ConversionProgress(object sender, ConvertProgressEventArgs e)
        {
            ConversionProgressbar.Value = (double)e.ProcessedDuration.Ticks / (double)e.TotalDuration.Ticks * 100;
        }

        public void Start()
        {
            IsEnabled = false;
            _eventAggregator.PublishOnUIThread(false);

            var selectedMovies = Movies.Where(x => x.IsSelected).OrderBy(x => x.Title).ToList();
            _eventAggregator.PublishOnUIThread(selectedMovies.Select(x => x.Title));

            Task.Run(() =>
            {
                _cts = new CancellationTokenSource();
                NotifyOfPropertyChange(() => CanStop);

                foreach (var movie in selectedMovies)
                {
                    Task task = _downloadFactory.StartNew(() => DownloadAsync(movie), _cts.Token);
                    _downloadTasks.Add(task);

                    //if (_cts.Token.IsCancellationRequested) //ToDo: Geht nicht
                    //    break;
                }

                Task.WaitAll(_downloadTasks.ToArray());
                Task.WaitAll(_conversionTasks.ToArray());
                Task.WaitAll(_transferTasks.ToArray());

                _eventAggregator.PublishOnUIThread(CommandEnum.ProgressbarEnd);

                _cts.Dispose();
                _cts = null;

                DownloadProgressbar.Value = 0;
                ConversionProgressbar.Value = 0;
                TransferProgressbar.Value = 0;

                IsEnabled = true;
                _eventAggregator.PublishOnUIThread(true);
            });
        }

        private void DownloadAsync(IMovieFile movie)
        {
            _eventAggregator.PublishOnUIThread(CommandEnum.ProgressbarNext);

            try
            {
                movie.FilePath = $@"tmp\{movie.Title}.tts";
                _logger.Debug($"Download started for [{movie.FileLink}]-[{Path.GetFileName(movie.FilePath)}]");
                using (var completedEvent = new ManualResetEventSlim(false))
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, args) =>
                    {
                        DownloadProgressbar.Value = (double)args.BytesReceived / (double)movie.Size * 100;
                    };
                    client.DownloadFileCompleted += (sender, args) =>
                    {
                        completedEvent.Set();
                    };
                    client.DownloadFileAsync(movie.FileLink, movie.FilePath);
                    completedEvent.Wait();
                }
                _logger.Debug($"Download finished for [{movie.FileLink}]-[{Path.GetFileName(movie.FilePath)}]");

                Task task = _conversionFactory.StartNew(() => Convert(movie), _cts.Token);
                _conversionTasks.Add(task);
            }
            catch (Exception ex)
            {
                _logger.Error($"Download failed for [{movie.FileLink}]-[{Path.GetFileName(movie.FilePath)}]", ex);
            }
        }

        private void Convert(IMovieFile movie)
        {
            var targetPath = Path.ChangeExtension(movie.FilePath, ".mp4");
            try
            {
                _logger.Debug($"Conversion started for [{Path.GetFileName(movie.FilePath)}]-[{Path.GetFileName(targetPath)}]");
                _ffmpeg.Convert(movie.FilePath, targetPath, _conversionOptions);
                _logger.Debug($"Conversion finished for [{Path.GetFileName(movie.FilePath)}]-[{Path.GetFileName(targetPath)}]");

                File.Delete(movie.FilePath);
                _logger.Debug($"Deleted [{Path.GetFileName(movie.FilePath)}]");
                movie.FilePath = targetPath;

                Task task = _transferFactory.StartNew(() => Transfer(movie), _cts.Token);
                _transferTasks.Add(task);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Conversion / Deletion failed for [{Path.GetFileName(movie.FilePath)}]-[{Path.GetFileName(targetPath)}]");
            }
        }

        private void Transfer(IMovieFile movie)
        {
            var fileName = Path.GetFileName(movie.FilePath);
            var targetPath = Path.Combine(@"N:\Movies\ALLE_RECS", fileName);
            try
            {
                _logger.Debug($"Move started for [{Path.GetFileName(movie.FilePath)}]-[{Path.GetFileName(targetPath)}]");

                var sourceInfo = new FileInfo(movie.FilePath);
                var targetInfo = new FileInfo(targetPath);
                sourceInfo.CopyTo(targetInfo, x => TransferProgressChanged(x));
                sourceInfo.Delete();

                _logger.Debug($"Move finished for [{Path.GetFileName(movie.FilePath)}]-[{Path.GetFileName(targetPath)}]");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Move failed for [{Path.GetFileName(movie.FilePath)}]-[{Path.GetFileName(targetPath)}]");
            }
        }

        private void TransferProgressChanged(int value)
        {
            TransferProgressbar.Value = value;
        }

        public void Stop()
        {
            _cts.Cancel();
        }
    }
}
