using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Ninject;
using PanasonicSync.GUI.Messaging.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationsCore;

namespace PanasonicSync.GUI.ViewModels
{
    public abstract class ViewModelBase : Screen
    {
        private static readonly HashSet<ViewModelBase> _viewModels = new HashSet<ViewModelBase>();
        internal readonly IDialogCoordinator _dialogCoordinator;
        internal readonly IKernel _standardKernel;
        internal readonly IWindowManager _manager;

        private TranslationProvider _translationProvider;

        public TranslationProvider TranslationProvider
        {
            get => _translationProvider;
            set
            {
                _translationProvider = value;
                NotifyOfPropertyChange();
            }
        }

        public ViewModelBase()
        {
            _standardKernel = Controller.Kernel;
            _manager = new WindowManager();
            TranslationProvider = Controller.TranslationProvider;
            _dialogCoordinator = DialogCoordinator.Instance;
            _viewModels.Add(this);
        }

        ~ViewModelBase()
        {
            _viewModels.Remove(this);
        }

#pragma warning disable CA1822 // Mark members as static
        public void SendMessage(IMessage message)
#pragma warning restore CA1822 // Mark members as static
        {
            foreach (var handler in _viewModels.OfType<IHandleMessage>())
            {
                handler.Handle(message);
            }
        }
    }
}
