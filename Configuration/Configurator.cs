using Configuration.Contracts;
using Configuration.Properties;
using DataStoring.Contracts;
using NetStandard.IO.Compression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Configuration
{
    public class Configurator : IConfigurator
    {
        private readonly Dictionary<object, object> _applicationSettings;
        private readonly Settings _appSettings;
        private readonly ICompressor _compressor;

        public Configurator(ICompressor compressor)
        {
            _appSettings = Settings.Default;
            _appSettings.PropertyChanged += SaveSettings;
            if (string.IsNullOrWhiteSpace(_appSettings.PathToConfigFile))
            {
                _appSettings.PathToConfigFile = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    _appSettings.AppName, _appSettings.ConfigFileName);
            }
            _applicationSettings = new Dictionary<object, object>();

            _compressor = compressor;

            if (File.Exists(_appSettings.PathToConfigFile))
                Set((ISettings)compressor.DeCompress<DataStoring.Settings>(File.ReadAllBytes(_appSettings.PathToConfigFile)));
        }

        private void SaveSettings(object sender, PropertyChangedEventArgs e)
        {
            _appSettings.Save();
        }

        public void Save()
        {
            File.WriteAllBytes(_appSettings.PathToConfigFile, _compressor.Compress(Get<ISettings>()));
        }

        public T Get<T>() where T : class
        {
            if (!_applicationSettings.ContainsKey(typeof(T)))
            {
                return null;
            }

            return (T)_applicationSettings[typeof(T)];
        }

        public void Set<T>(T Setting) where T : class
        {
            if (_applicationSettings.ContainsKey(typeof(T)))
            {
                _applicationSettings.Remove(typeof(T));
            }

            _applicationSettings.Add(typeof(T), Setting);
        }
    }
}
