using Caliburn.Micro;
using Configuration.Contracts;
using DataStoring;
using GUI.ViewModels;
using NetStandard.Logger;
using Ninject;
using System;
using System.Windows;

namespace GUI
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            IConfigurator configurator = Controller.Kernel.Get<IConfigurator>();
            configurator.Load(new Type[] { typeof(Settings) });
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            ILoggerFactory loggerFactory = Controller.Kernel.Get<ILoggerFactory>();
            loggerFactory.Dispose();
            base.OnExit(sender, e);
        }
    }
}
