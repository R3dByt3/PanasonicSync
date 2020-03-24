﻿using APIClient.Contracts.Panasonic;
using Caliburn.Micro;
using DataStoring.Contracts;
using DataStoring.Contracts.MovieModels;
using DataStoring.Contracts.UpnpResponse;
using FFmpegStandardWrapper.Abstract.Core;
using MahApps.Metro.Controls.Dialogs;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.Comparer;
using PanasonicSync.GUI.Messaging.Impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranslationsCore;

namespace PanasonicSync.GUI.ViewModels
{
    public class SyncSelectionViewModel : ViewModelBase
    {
        private TranslationProvider _translationProvider;
        private IPanasonicDevice _panasonicDevice;

        private readonly ILogger _logger;
        private readonly IKernel _standardKernel;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly ISettings _settings;

        public IPanasonicDevice PanasonicDevice 
        { 
            get => _panasonicDevice;
            set
            {
                _panasonicDevice = value;
                NotifyOfPropertyChange();
            }
        }

        public TranslationProvider TranslationProvider
        {
            get => _translationProvider;
            set
            {
                _translationProvider = value;
                NotifyOfPropertyChange();
            }
        }

        public SyncSelectionViewModel(IPanasonicDevice panasonicDevice)
        {
            PanasonicDevice = panasonicDevice;
            _standardKernel = Controller.Kernel;
            TranslationProvider = Controller.TranslationProvider;
            _settings = _standardKernel.Get<ISettings>();
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();
            _dialogCoordinator = DialogCoordinator.Instance;

            Load(panasonicDevice);
        }

        private async void Load(IPanasonicDevice panasonicDevice)
        {
            var panasonicClient = _standardKernel.Get<IPanasonicClient>();
            panasonicClient.LoadControlsUri(panasonicDevice);
            SendMessage(new SetProgressControlMessage(new ProgressbarViewModel(new[]
            {
                new Tuple<string, bool>(TranslationProvider.CompareMovies, true),
                new Tuple<string, bool>(TranslationProvider.LoadingMovies, true),
            })));
            SendMessage(new ProgressbarNextMessage());

            var remoteItems = Task.Run(() => panasonicClient.RequestMovies().ToList());
            var localItems = Task.Run(() => LoadLocalFiles().ToList());

            var remoteMovies = await remoteItems;
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

            SendMessage(new ProgressbarNextMessage());

            var equalityComparer = new MovieEqualityComparer();
            var similarityComparer = new MovieSimilarityComparer();
            var missingMovies = remoteFileList.Where(x => !localMovies.Contains(x, equalityComparer)).ToList();

            HashSet<IConflict> conflicts = new HashSet<IConflict>();

            foreach(var movie in missingMovies)
            {
                var conflicted = localMovies.Where(x => similarityComparer.Equals(x, movie)).ToList();
                if (!conflicted.Any())
                    continue;

                var conflict = _standardKernel.Get<IConflict>();
                conflict.MovieFile = movie;
                conflict.Conflicts = conflicted;

                conflicts.Add(conflict);
            }

            var duplicates = conflicts.Select(x => x.MovieFile).Distinct();
            missingMovies = missingMovies.Where(x => !duplicates.Contains(x, equalityComparer)).ToList();

            var localDuplicates = conflicts.SelectMany(x => x.Conflicts).Distinct().ToList();

            var probe = _standardKernel.Get<IFFprobe>();

            Parallel.ForEach(localDuplicates, local =>
            {
                local.Duration = probe.GetVideoLenght(local.FilePath);
            });

            SendMessage(new ProgressbarEndMessage());
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
    }
}