using Caliburn.Micro;
using DataStoring.Contracts.UpnpResponse;
using MahApps.Metro.Controls.Dialogs;
using NetStandard.Logger;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslationsCore;
using UpnpClient.Contracts;

namespace PanasonicSync.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen, IHandle<ViewModelBase>
    {
        private readonly ILogger _logger;

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

        public MainWindowViewModel()
        {
            TranslationProvider = Controller.TranslationProvider;
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();

            ProgressModel = new ProgressbarViewModel
            {
                IsIndeterminate = true
            };
            ProgressModel.SetSteps(new[] { TranslationProvider.SearchForDevices });
            ProgressModel.Next();
        }

        public void Loaded()
        {
            //_manager.ShowDialog(new LoadingScreenViewModel(_standardKernel), this, new Dictionary<string, object> { { "" } });

            Task.Run(async () =>
            {
                _logger.Info($"Device detection started");
                var devices = await StartDeviceDetectionLoop();
                _logger.Info($"[{devices.Count}] Devices found");

                if (devices.Count > 1)
                    CurrentModel = new DeviceSelectionViewModel(devices);
                else
                    CurrentModel = new SyncSelectionViewModel(devices.First());
            });
        }

        private async Task<List<IPanasonicDevice>> StartDeviceDetectionLoop()
        {
            var devices = SearchDevices();

            while (!devices.Any())
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
            IClient client = _standardKernel.Get<IClient>();
            var devices = client.SearchUpnpDevices().ToList();
            ProgressModel.End();

            return devices;
        }

        public void Handle(ViewModelBase message)
        {
            CurrentModel = message;
        }
    }
}
