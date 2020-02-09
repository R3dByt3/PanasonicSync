using DataStoring.Contracts.UpnpResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpnpClient.Contracts
{
    public interface IClient
    {
        string LocalIP { get; }

        IEnumerable<IPanasonicDevice> SearchUpnpDevices();
    }
}
