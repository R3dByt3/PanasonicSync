using DataStoring.Contracts.UpnpResponse;
using DataStoring.PanasonicResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient.Contracts.Panasonic
{
    public interface IPanasonicClient
    {
        void LoadControlsUri(IPanasonicDevice device);
        IEnumerable<Item> RequestMovies();
    }
}
