using DataStoring.Contracts;

namespace DataStoring
{
    public class Settings : ISettings
    {
        public double DeviceDiscoveringTime { get; set; }
        public string LocalMoviesPath { get; set; }
    }
}
