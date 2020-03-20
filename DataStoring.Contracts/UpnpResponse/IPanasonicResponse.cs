using System;

namespace DataStoring.Contracts.UpnpResponse
{
    public interface IPanasonicDevice
    {
        bool IsChecked { get; set; }
        string Location { get; set; }
        string ServiceType { get; set; }
        DateTime LastSeen { get; set; }
        string UUID { get; set; }

        void FillFromString(string input);
    }
}
