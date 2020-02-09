using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoring.Contracts.UpnpResponse
{
    public interface IPanasonicDevice
    {
        string Location { get; set; }
        string ServiceType { get; set; }
        DateTime LastSeen { get; set; }
        string UUID { get; set; }

        void FillFromString(string input);
    }
}
