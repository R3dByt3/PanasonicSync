using DataStoring.Contracts.UpnpResponse;
using System.Collections.Generic;

namespace UpnpClient.Contracts
{
    public interface IClient
    {
        List<string> LocalIPs { get; }

        IEnumerable<IPanasonicDevice> SearchUpnpDevices();
    }
}
