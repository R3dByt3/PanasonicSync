using APIClient.Contracts.Panasonic;
using Caliburn.Micro;
using DataStoring.Contracts.UpnpResponse;
using MahApps.Metro.Controls.Dialogs;
using NetStandard.Logger;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranslationsCore;
using UpnpClient.Contracts;

namespace PanasonicSync.GUI.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        public readonly TranslationProvider TranslationProvider;

        private readonly ILogger _logger;
        private readonly IKernel _standardKernel;
        private readonly IDialogCoordinator _dialogCoordinator;

        private object _currentModel;
        private ProgressbarViewModel _progressModel;

        public object CurrentModel
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
            _standardKernel = Controller.Kernel;
            TranslationProvider = Controller.TranslationProvider;
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();
            _dialogCoordinator = DialogCoordinator.Instance;

            Thread thread = new Thread(() => {
                var devices = StartDeviceDetectionLoop();

                CurrentModel = new DeviceSelectionViewModel(devices);
            });
            thread.Start();
        }

        private List<IPanasonicDevice> StartDeviceDetectionLoop()
        {
            List<IPanasonicDevice> devices = SearchDevices();

            while (!devices.Any()) //ToDo: Test
            {
                devices = SearchDevices();
                var result = _dialogCoordinator.ShowMessageAsync(
                    this, 
                    TranslationProvider.Error, 
                    TranslationProvider.NoDevicesFoundRetry, 
                    MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = TranslationProvider.Retry,
                        NegativeButtonText = TranslationProvider.Close
                    }).Result;

                if (result == MessageDialogResult.Canceled)
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
    }
}
