using Caliburn.Micro;
using DataStoring.Contracts;
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
    public class MainWindowViewModel : ViewModelBase, IScreen, IHandle<ViewModelBase>, IHandle<bool>
    {
        private readonly ILogger _logger;

        private bool _isEnabled;
        private ViewModelBase _currentModel;
        private ProgressbarViewModel _progressModel;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange();
            }
        }

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
            IsEnabled = true;
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();

            ProgressModel = new ProgressbarViewModel();
        }

        protected override void OnViewLoaded(object view)
        {
            var settings = _standardKernel.Get<ISettings>();

            if (!settings.IsValid)
                OpenSettings();

            base.OnViewLoaded(view);
        }

        public void Start()
        {
            if (CurrentModel is ConfigurationViewModel configurationViewModel)
                configurationViewModel.TryClose();

            var settings = _standardKernel.Get<ISettings>();

            if (!settings.IsValid)
            {
                OpenSettings();
                return;
            }

            ProgressModel.IsIndeterminate = true;
            ProgressModel.SetSteps(new[] { TranslationProvider.SearchForDevices });
            ProgressModel.Next();

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
            List<IPanasonicDevice> devices = new List<IPanasonicDevice>();

            IClient client = _standardKernel.Get<IClient>();
            try
            {
                devices = client.SearchUpnpDevices().ToList();
            }
            catch
            {
                return new List<IPanasonicDevice>();
            }
            ProgressModel.End();

            return devices;
        }

        public void OpenSettings()
        {
            CurrentModel = new ConfigurationViewModel();
        }

        public void Handle(ViewModelBase message)
        {
            CurrentModel = message;
        }

        public void Handle(bool message)
        {
            IsEnabled = message;
        }
    }
}
