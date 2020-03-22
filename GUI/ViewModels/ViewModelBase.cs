using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using PanasonicSync.GUI.Messaging.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.ViewModels
{
    public abstract class ViewModelBase : PropertyChangedBase
    {
        private static readonly HashSet<ViewModelBase> _viewModels = new HashSet<ViewModelBase>();
        internal readonly IDialogCoordinator _dialogCoordinator;

        public ViewModelBase()
        {
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
