using PanasonicSync.GUI.Messaging.Abstract;
using PanasonicSync.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.Messaging.Impl
{
    public class SetMainWindowControlMessage : IMessage
    {
        public ViewModelBase ViewModel { get; }

        public SetMainWindowControlMessage(ViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
