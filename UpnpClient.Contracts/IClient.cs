using DataStoring.Contracts.UpnpResponse;
using System.Collections.Generic;

namespace UpnpClient.Contracts
{
    public interface IClient
    {
        string LocalIP { get; }

        IEnumerable<IPanasonicDevice> SearchUpnpDevices();
    }
}
