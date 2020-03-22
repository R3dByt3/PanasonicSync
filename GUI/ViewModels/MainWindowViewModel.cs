using APIClient.Contracts.Panasonic;
using Caliburn.Micro;
using DataStoring.Contracts.UpnpResponse;
using MahApps.Metro.Controls.Dialogs;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.Messaging.Abstract;
using PanasonicSync.GUI.Messaging.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranslationsCore;
using UpnpClient.Contracts;

namespace PanasonicSync.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IHandleMessage
    {
        private TranslationProvider _translationProvider;

        private readonly ILogger _logger;
        private readonly IKernel _standardKernel;

        private ViewModelBase _currentModel;
        private ProgressbarViewModel _progressModel;

        public ViewModelBase CurrentModel
        {
            get => _currentModel;
            set 
            { 
                _currentModel = value; 
                NotifyOfPropertyChange(); 
            }
        }

        public ProgressbarViewModel ProgressModel
        {
            get => _progressModel;
            set
            {
                _progressModel = value;
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

        public MainWindowViewModel()
        {
            _standardKernel = Controller.Kernel;
            TranslationProvider = Controller.TranslationProvider;
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();
        }

        public async void Loaded()
        {
            await Task.Run(async () =>
            {
                var devices = await StartDeviceDetectionLoop();

                if (devices.Count > 1)
                    CurrentModel = new DeviceSelectionViewModel(devices);
                else
                    CurrentModel = new SyncSelectionViewModel(devices.First());
            });
        }

        private async Task<List<IPanasonicDevice>> StartDeviceDetectionLoop()
        {
            var devices = SearchDevices();

            while (!devices.Any()) //ToDo: Test
            {
                devices = SearchDevices();
                var result = await _dialogCoordinator.ShowMessageAsync(
                    this, 
                    TranslationProvider.Error, 
                    TranslationProvider.NoDevicesFoundRetry, 
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = TranslationProvider.Retry,
                        NegativeButtonText = TranslationProvider.Close
                    });

                if (result == MessageDialogResult.Negative)
                {
                    Environment.Exit(0);
                }
            }

            return devices;
        }

        private List<IPanasonicDevice> SearchDevices()
        {
            ProgressModel = new ProgressbarViewModel(new[]
            {
                new Tuple<string, bool>(TranslationProvider.SearchForDevices, true),
            });

            ProgressModel.Next();
            IClient client = _standardKernel.Get<IClient>();
            var devices = client.SearchUpnpDevices().ToList();
            ProgressModel.End();

            return devices;
        }

        public void Handle(IMessage message)
        {
            if (message is ProgressbarEndMessage)
                ProgressModel.End();
            else if (message is ProgressbarNextMessage)
                ProgressModel.Next();
            else if (message is SetMainWindowControlMessage setControlMessage)
                CurrentModel = setControlMessage.ViewModel;
            else if (message is SetProgressControlMessage setProgessMessage)
                ProgressModel = setProgessMessage.ViewModel;
        }
    }
}
