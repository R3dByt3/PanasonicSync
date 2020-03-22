using Caliburn.Micro;
using DataStoring.Contracts.UpnpResponse;
using MahApps.Metro.Controls.Dialogs;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.Messaging.Abstract;
using PanasonicSync.GUI.Messaging.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TranslationsCore;

namespace PanasonicSync.GUI.ViewModels
{
    public class DeviceSelectionViewModel : ViewModelBase
    {
        private TranslationProvider _translationProvider;

        private readonly ILogger _logger;
        private readonly IKernel _standardKernel;

        private IObservableCollection<IPanasonicDevice> _devices;

        public IObservableCollection<IPanasonicDevice> Devices 
        { 
            get => _devices; 
            set
            {
                _devices = value;
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

        public DeviceSelectionViewModel(List<IPanasonicDevice> devices)
        {
            devices[0].IsChecked = true;
            Devices = new BindableCollection<IPanasonicDevice>(devices);
            _standardKernel = Controller.Kernel;
            TranslationProvider = Controller.TranslationProvider;
            var factory = _standardKernel.Get<ILoggerFactory>();
            _logger = factory.CreateFileLogger();
        }

        public void Select()
        {
            SendMessage(new SetMainWindowControlMessage(new SyncSelectionViewModel(Devices.First(x => x.IsChecked))));
        }
    }
}
