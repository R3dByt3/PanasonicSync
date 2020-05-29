using Caliburn.Micro;
using Configuration.Contracts;
using DataStoring;
using DataStoring.Contracts;
using FFmpegStandardWrapper.Abstract.Core;
using NetStandard.Logger;
using Ninject;
using PanasonicSync.GUI.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => OnUnhandledException(sender, e);
            Application.DispatcherUnhandledException += (sender, e) => OnUnhandledException(sender, e);
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            var kernel = Controller.Kernel;
            var factory = kernel.Get<ILoggerFactory>();
            var configurator = kernel.Get<IConfigurator>();

            kernel.Bind<ISettings>().ToMethod(x => configurator.Get<ISettings>() ?? new Settings())
                .InSingletonScope();

#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var engine = Controller.Kernel.Get<IEngine>();
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            _logger = factory.CreateFileLogger();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            var factory = Controller.Kernel.Get<ILoggerFactory>();
            factory.Dispose();
            base.OnExit(sender, e);
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

        protected void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                LogErrors(e.ExceptionObject as Exception);
                Xceed.Wpf.Toolkit.MessageBox.Show("Ein unbekannter Fehler ist aufgetreten. Bitte schließen Sie das Programm und kontaktieren Sie den Entwickler.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        protected void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                LogErrors(e.Exception);
                Xceed.Wpf.Toolkit.MessageBox.Show("Ein unbekannter Fehler ist aufgetreten. Bitte schließen Sie das Programm und kontaktieren Sie den Entwickler.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                e.SetObserved();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
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
