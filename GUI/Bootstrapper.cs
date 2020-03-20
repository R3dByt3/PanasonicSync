using Caliburn.Micro;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.ViewModels;
using System;
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
