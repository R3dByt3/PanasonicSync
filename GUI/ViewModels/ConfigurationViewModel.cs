using Caliburn.Micro;
using Configuration.Contracts;
using DataStoring.Contracts;
using Ninject;
using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using static System.Environment;

namespace PanasonicSync.GUI.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase, IScreen
    {
        private readonly IConfigurator _configurator;
        private readonly ISettings _settings;

        private bool _changed;

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
            _settings = _standardKernel.Get<ISettings>();
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
            if (_settings.DeviceDiscoveringTime == 0)
            {
                _settings.DeviceDiscoveringTime = 1;
                _settings.LocalMoviesPath = string.Empty;
                _settings.BlackList = new List<string>();
            }

            DeviceDiscoveringTime = _settings.DeviceDiscoveringTime;
            LocalMoviesPath = _settings.LocalMoviesPath;
            BlackListArray = string.Join(";", _settings.BlackList);

            base.OnViewLoaded(view);
        }

        public void Save()
        {
            _settings.DeviceDiscoveringTime = DeviceDiscoveringTime;
            _settings.LocalMoviesPath = LocalMoviesPath;

            if (!string.IsNullOrWhiteSpace(BlackListArray))
                _settings.BlackList = BlackListArray.Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (!_settings.IsValid)
                return;

            if (_changed)
            {
                _configurator.Set(_settings);
                _configurator.Save();
                _changed = false;
            }
        }

        public override void TryClose(bool? dialogResult = null)
        {
            Save();
            base.TryClose(dialogResult);
        }
    }
}
