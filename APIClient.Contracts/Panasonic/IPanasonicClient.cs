using DataStoring.Contracts.PanasonicResponse;
using DataStoring.Contracts.UpnpResponse;
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
        IEnumerable<IItem> RequestMovies();
    }
}
