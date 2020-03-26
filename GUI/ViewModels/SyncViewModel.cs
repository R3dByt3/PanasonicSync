using Caliburn.Micro;
using DataStoring.Contracts.MovieModels;
using FFmpegStandardWrapper.Abstract.Core;
using FFmpegStandardWrapper.Abstract.Options;
using Ninject;
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
            Movies = new BindableCollection<IMovieFile>(movies);

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
                using (var client = new WebClient())
                {
                    client.DownloadFile(movie.FileLink, movie.FilePath);
                }

                Task task = _conversionFactory.StartNew(() => Convert(movie), _cts.Token);
                _conversionTasks.Add(task);
            }
            catch (Exception ex)
            {

            }
        }

        private void Convert(IMovieFile movie)
        {
            try
            {
                var info = _ffprobe.GetDetailedMovieInformation(movie.FilePath);
                var newPath = Path.ChangeExtension(movie.FilePath, ".mp4");
                _ffmpeg.Convert(info, newPath, _conversionOptions);
                movie.FilePath = newPath;

                Task task = _transferFactory.StartNew(() => Transfer(movie), _cts.Token);
                _transferTasks.Add(task);
            }
            catch (Exception ex)
            {

            }
        }

        private void Transfer(IMovieFile movie)
        {
            var fileName = Path.GetFileName(movie.FilePath);
            var targetPath = Path.Combine(@"N:\Movies\ALLE_RECS", fileName);
            File.Move(movie.FilePath, fileName);
        }
    }
}
