﻿using Caliburn.Micro;
using Configuration.Contracts;
using DataStoring.Contracts;
using FFmpegStandardWrapper.Abstract.Core;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace PanasonicSync.GUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly ILogger _logger;

        public Bootstrapper()
        {
            Initialize();
            var factory = Controller.Kernel.Get<ILoggerFactory>();
            var configurator = Controller.Kernel.Get<IConfigurator>();
            var settings = configurator.Get<ISettings>();

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var engine = Controller.Kernel.Get<IEngine>();
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            if (settings == null)
            {
                settings = Controller.Kernel.Get<ISettings>();
                settings.DeviceDiscoveringTime = 1;
                settings.LocalMoviesPath = @"N:\Movies\ALLE_RECS";
                settings.BlackList = new List<string> { "Club der roten Bänder" };
                configurator.Set(settings);
                configurator.Save();
            }

            _logger = factory.CreateFileLogger();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            LogManager.GetLog = type => new DebugLogger(type);
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogErrors(e.Exception);
            Xceed.Wpf.Toolkit.MessageBox.Show("Ein unbekannter Fehler ist aufgetreten. Bitte schließen Sie das Programm und kontaktieren Sie den Entwickler.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;

            base.OnUnhandledException(sender, e);
        }

        private void LogErrors(Exception ex)
        {
            if (ex == null)
                return;

            _logger.Fatal("Unhandled exception occured", ex);

            if (ex.InnerException != null)
                LogErrors(ex.InnerException);
        }
    }
}
