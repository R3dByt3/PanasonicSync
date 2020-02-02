using Configuration.Contracts;
using DataStorage.Contracts;
using DataStoring.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Configuration
{
    public class Configurator : IConfigurator
    {
        private readonly Dictionary<object, object> _applicationSettings;
        private readonly IDatabaseAccess _databaseAccess;
        private readonly Properties.Settings _appSettings;

        public Configurator(IDatabaseAccess databaseAccess)
        {
            _appSettings = Properties.Settings.Default;
            _appSettings.PropertyChanged += SaveSettings;
            if (string.IsNullOrWhiteSpace(_appSettings.PathToDb))
            {
                _appSettings.PathToDb = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    _appSettings.AppName, _appSettings.DbName);
            }
            _databaseAccess = databaseAccess;
            _applicationSettings = new Dictionary<object, object>();
        }

        private void SaveSettings(object sender, PropertyChangedEventArgs e)
        {
            _appSettings.Save();
        }

        public void Load(IEnumerable<Type> types)
        {
            _databaseAccess.InitDBA(_appSettings.PathToDb);
            _databaseAccess.InsertTables(types);
            IList<ISettings> settings = _databaseAccess.GetAll<ISettings>().ToList();
            if (settings != null && settings.Count != 0)
            {
                Set(settings.First());
            }
            else
            {
                Set((ISettings)null);
            }
        }

        public void Save()
        {
            _databaseAccess.SaveObject(Get<ISettings>());
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
