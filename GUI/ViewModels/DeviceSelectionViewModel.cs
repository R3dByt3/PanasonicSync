using Caliburn.Micro;
using DataStoring.Contracts.UpnpResponse;
using System.Collections.Generic;
using System.Linq;

namespace PanasonicSync.GUI.ViewModels
{
    public class DeviceSelectionViewModel : ViewModelBase, IScreen
    {
        //private readonly ILogger _logger;

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

        public DeviceSelectionViewModel(List<IPanasonicDevice> devices)
        {
            devices[0].IsChecked = true;
            Devices = new BindableCollection<IPanasonicDevice>(devices);
            //var factory = _standardKernel.Get<ILoggerFactory>();
            //_logger = factory.CreateFileLogger();
        }

        public void Select()
        {
            _eventAggregator.PublishOnUIThread(new SyncSelectionViewModel(Devices.First(x => x.IsChecked)));
        }
    }
}
