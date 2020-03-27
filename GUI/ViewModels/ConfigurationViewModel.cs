using Caliburn.Micro;
using Configuration.Contracts;
using DataStoring.Contracts;
using Ninject;
using Ookii.Dialogs.Wpf;
using static System.Environment;

namespace PanasonicSync.GUI.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase, IScreen
    {
        private readonly IConfigurator _configurator;

        private bool _changed;
        private ISettings _settings;

        private double _deviceDiscoveringTime;
        private string _localMoviesPath;
        private string _blackListArray;

        public double DeviceDiscoveringTime 
        {
            get => _deviceDiscoveringTime;
            set
            {
                _deviceDiscoveringTime = value;
                _changed = true;
                NotifyOfPropertyChange();
            }
        }

        public string LocalMoviesPath
        {
            get => _localMoviesPath;
            set
            {
                _localMoviesPath = value;
                _changed = true;
                NotifyOfPropertyChange();
            }
        }

        public string BlackListArray
        {
            get => _blackListArray;
            set
            {
                _blackListArray = value;
                _changed = true;
                NotifyOfPropertyChange();
            }
        }

        public ConfigurationViewModel()
        {
            _configurator = _standardKernel.Get<IConfigurator>();
            _settings = _configurator.Get<ISettings>();
        }

        public void OpenFolder()
        {
            var dialog = new VistaFolderBrowserDialog
            {
                Description = TranslationProvider.LocalMoviesPathHeader,
                RootFolder = SpecialFolder.MyComputer,
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true
            };
            if (!dialog.ShowDialog() ?? false)
                return;

            LocalMoviesPath = dialog.SelectedPath;
        }

        protected override void OnViewLoaded(object view)
        {
            if (_settings == null)
            {
                _settings = _standardKernel.Get<ISettings>();
                DeviceDiscoveringTime = 1;
            }
            else
            {
                DeviceDiscoveringTime = _settings.DeviceDiscoveringTime;
                LocalMoviesPath = _settings.LocalMoviesPath;
                BlackListArray = string.Join(";", _settings.BlackList);
            }

            base.OnViewLoaded(view);
        }

        public void Save()
        {
            _settings.DeviceDiscoveringTime = DeviceDiscoveringTime;
            _settings.LocalMoviesPath = LocalMoviesPath;

            if (!string.IsNullOrWhiteSpace(BlackListArray))
                _settings.BlackList = BlackListArray.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (_changed)
            {
                _configurator.Set(_settings);
                _configurator.Save();
                _changed = false;
            }
        }
    }
}
