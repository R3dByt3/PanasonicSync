using DataStoring.Contracts;
using System.Collections.Generic;
using System.IO;

namespace DataStoring
{
    public class Settings : ISettings
    {
        public double DeviceDiscoveringTime { get; set; }
        public string LocalMoviesPath { get; set; }
        public IList<string> BlackList { get; set; }

        public bool IsValid => BlackList != null && 
            !string.IsNullOrWhiteSpace(LocalMoviesPath) && 
            Directory.Exists(LocalMoviesPath) && 
            DeviceDiscoveringTime > 0;
    }
}
