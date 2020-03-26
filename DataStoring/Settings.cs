using DataStoring.Contracts;
using System.Collections.Generic;

namespace DataStoring
{
    public class Settings : ISettings
    {
        public double DeviceDiscoveringTime { get; set; }
        public string LocalMoviesPath { get; set; }
        public IList<string> BlackList { get; set; }
    }
}
