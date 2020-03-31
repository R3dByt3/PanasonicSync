using System.Collections.Generic;

namespace DataStoring.Contracts
{
    public interface ISettings
    {
        double DeviceDiscoveringTime { get; set; }
        string LocalMoviesPath { get; set; }
        IList<string> BlackList { get; set; }
        bool IsValid { get; }
    }
}
