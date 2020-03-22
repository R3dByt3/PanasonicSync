using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanasonicSync.GUI.Messaging.Abstract
{
    public interface IHandleMessage
    {
        void Handle(IMessage message);
    }
}
