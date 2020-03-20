using DataStoring.Contracts.UpnpResponse;
using DataStoring.PanasonicResponse;
using System.Collections.Generic;

namespace APIClient.Contracts.Panasonic
{
    public interface IPanasonicClient
    {
        void LoadControlsUri(IPanasonicDevice device);
        IEnumerable<Item> RequestMovies();
    }
}
