using Caliburn.Micro;
using DataStoring.Contracts.UpnpResponse;
using MahApps.Metro.Controls.Dialogs;
using NetStandard.Logger;
using Ninject;
using System.Collections.Generic;
using TranslationsCore;

namespace PanasonicSync.GUI.ViewModels
{
    public class DeviceSelectionViewModel : PropertyChangedBase
    {
        public readonly TranslationProvider TranslationProvider;

        private readonly ILogger _logger;
        private readonly IKernel _standardKernel;
        private readonly IDialogCoordinator _dialogCoordinator;

        private List<IPanasonicDevice> _devices;

        public List<IPanasonicDevice> Devices 
        { 
            get => _devices; 
            set
            {
                _devices = value;
                NotifyOfPropertyChange();
            }
        }

        public DeviceSelectionViewModel(List<IPanasonicDevice> devices)
        {
            Devices = devices;
            _standardKernel = Controller.Kernel;
            TranslationProvider = Controller.TranslationProvider;
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();
            _dialogCoordinator = DialogCoordinator.Instance;
        }
    }
}
