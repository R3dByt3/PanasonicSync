using APIClient.Contracts.Panasonic;
using Caliburn.Micro;
using DataStoring.Contracts;
using DataStoring.Contracts.MovieModels;
using DataStoring.Contracts.UpnpResponse;
using FFmpegStandardWrapper.Abstract.Core;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.Comparer;
using PanasonicSync.GUI.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranslationsCore;

namespace PanasonicSync.GUI.ViewModels
{
    public class SyncSelectionViewModel : ViewModelBase, IScreen
    {
        private IPanasonicDevice _panasonicDevice;
        private Stack<IConflict> _conflictsToResolve;

        private readonly ILogger _logger;
        private readonly ISettings _settings;
        private IMovieFile _movie;
        private IList<MovieViewModel> _conflicts;
        private IList<IMovieFile> _missingMovies;
        private bool _isEnabled;

        public IMovieFile Movie
        {
            get => _movie;
            set
            {
                _movie = value;
                NotifyOfPropertyChange();
            }
        }

        public IList<MovieViewModel> Conflicts
        {
            get => _conflicts;
            set
            {
                _conflicts = value;
                NotifyOfPropertyChange();
            }
        }

        public IPanasonicDevice PanasonicDevice 
        { 
            get => _panasonicDevice;
            set
            {
                _panasonicDevice = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange();
            }
        }

        public SyncSelectionViewModel(IPanasonicDevice panasonicDevice)
        {
            PanasonicDevice = panasonicDevice;
            TranslationProvider = Controller.TranslationProvider;
            _settings = _standardKernel.Get<ISettings>();
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();

            Load(panasonicDevice);
        }

        private async void Load(IPanasonicDevice panasonicDevice)
        {
            var panasonicClient = _standardKernel.Get<IPanasonicClient>();
            _logger.Info($"Attempting to gain controls for device [{panasonicDevice}]");
            panasonicClient.LoadControlsUri(panasonicDevice);
            _logger.Info($"Controls gained for device [{panasonicDevice}]");

            _eventAggregator.PublishOnUIThread(new[] { TranslationProvider.LoadingMovies, TranslationProvider.CompareMovies });
            _eventAggregator.PublishOnUIThread(CommandEnum.IsIndetermined);
            _eventAggregator.PublishOnUIThread(CommandEnum.ProgressbarNext);

            _logger.Info($"Attempting to gain remote movies");
            var remoteItems = Task.Run(() => panasonicClient.RequestMovies().ToList());
            _logger.Info($"Attempting to gain local movies");
            var localItems = Task.Run(() => LoadLocalFiles().ToList());

            var remoteMovies = await remoteItems;
            _logger.Info($"[{remoteMovies.Count}] remote movies gained");
            List<IMovieFile> remoteFileList = new List<IMovieFile>();

            foreach (var file in remoteMovies)
            {
                var remoteFile = _standardKernel.Get<IMovieFile>();
                remoteFile.Title = RemoveInvalidChars(file.Title);
                remoteFile.Duration = TimeSpan.Parse(file.RemoteMovie.Duration);
                remoteFile.FileLink = new Uri(file.RemoteMovie.Text);
                remoteFile.Size = ulong.Parse(file.RemoteMovie.Size);
                remoteFileList.Add(remoteFile);
            }

            var localMovies = await localItems;
            _logger.Info($"[{localMovies.Count}] local movies gained");

            _eventAggregator.PublishOnUIThread(CommandEnum.ProgressbarNext);

            Compare(remoteFileList, localMovies);
        }

        private void Compare(List<IMovieFile> remoteFileList, List<IMovieFile> localMovies)
        {
            var equalityComparer = new MovieEqualityComparer();
            var similarityComparer = new MovieSimilarityComparer();
            _missingMovies = remoteFileList.Where(x => !localMovies.Contains(x, equalityComparer)).ToList();

            ISettings settings = _standardKernel.Get<ISettings>();

            foreach (var blacklisted in settings.BlackList)
            {
                _missingMovies = _missingMovies.Where(x => !x.Title.Contains(blacklisted)).ToList();
            }

            _conflictsToResolve = new Stack<IConflict>();

            foreach (var movie in _missingMovies)
            {
                var conflicted = localMovies.Where(x => similarityComparer.Equals(x, movie)).ToList();
                if (!conflicted.Any())
                    continue;

                var conflict = _standardKernel.Get<IConflict>();
                conflict.MovieFile = movie;
                conflict.Conflicts = conflicted;

                _conflictsToResolve.Push(conflict);
            }

            var duplicates = _conflictsToResolve.Select(x => x.MovieFile).Distinct();
            _missingMovies = _missingMovies.Where(x => !duplicates.Contains(x, equalityComparer)).ToList();

            var localDuplicates = _conflictsToResolve.SelectMany(x => x.Conflicts).Distinct().ToList();

            var probe = _standardKernel.Get<IFFprobe>();

            Parallel.ForEach(localDuplicates, local =>
            {
                try
                {
                    local.Duration = probe.GetVideoLenght(local.FilePath);
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, $"Could not get movie lenght for local [{local.Title}]");
                }
            });

            _eventAggregator.PublishOnUIThread(CommandEnum.ProgressbarEnd);
            IsEnabled = true;
            SetNextConflict();
        }

        private void SetNextConflict()
        {
            if (_conflictsToResolve == null || !_conflictsToResolve.Any())
            {
                _eventAggregator.PublishOnUIThread(new SyncViewModel(_missingMovies));
                return;
            }

            var nextConflict = _conflictsToResolve.Pop();

            Movie = nextConflict.MovieFile;
            Conflicts = nextConflict.Conflicts.Select(x => new MovieViewModel(x, true)).ToList();
        }

        private IEnumerable<IMovieFile> LoadLocalFiles()
        {
            foreach (var file in Directory.GetFiles(_settings.LocalMoviesPath))
            {
                var fileInfo = new FileInfo(file);

                var localFile = _standardKernel.Get<IMovieFile>();
                localFile.Title = Path.GetFileNameWithoutExtension(file);
                localFile.FilePath = file;
                localFile.Size = (ulong)fileInfo.Length;

                yield return localFile;
            }
        }

        private string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).ToArray()));
        }

        public void Skip()
        {
            SetNextConflict();
        }

        public void Download()
        {
            GetNextFreeTitle();
            _missingMovies.Add(Movie);
            SetNextConflict();
        }

        private void GetNextFreeTitle()
        {
            var title = Movie.Title;
            int counter = 0;
            while (_missingMovies.Any(x => x.Title == Movie.Title))
            {
                title = $"{title}_{counter}";
            }
            Movie.Title = title;
        }
    }
}
