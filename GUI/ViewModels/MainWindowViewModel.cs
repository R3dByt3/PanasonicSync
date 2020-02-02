using NetStandard.Logger;
using Ninject;

namespace GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ILoggerFactory _loggerFactory;
        private IKernel _standardKernel;

        //private VehicleViewModel _vehicleViewModel;
        //private EquipmentViewModel _equipmentViewModel;
        //private ComparerViewModel _comparerViewModel;

        private ViewModelBase _currentModel;
        public ViewModelBase CurrentModel
        {
            get => _currentModel;
            set { _currentModel = value; OnPropertyChanged(); }
        }

        public MainWindowViewModel()
        {
            _standardKernel = Controller.Kernel;
            _loggerFactory = _standardKernel.Get<ILoggerFactory>();
            //_vehicleViewModel = new VehicleViewModel();
            //_equipmentViewModel = new EquipmentViewModel();
            //_comparerViewModel = new ComparerViewModel();
            SwitchPage(0);
        }

        public void SwitchPage(int pageId)
        {
            //if (pageId == 0)
            //    CurrentModel = _vehicleViewModel;
            //else if (pageId == 1)
            //    CurrentModel = _equipmentViewModel;
            //else if (pageId == 2)
            //    CurrentModel = _comparerViewModel;
            //else
            //    throw new Exception("Tab not found");
        }
    }
}
