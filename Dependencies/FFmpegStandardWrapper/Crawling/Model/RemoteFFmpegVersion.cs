using System;

namespace FFmpegStandardWrapper.Crawling.Model
{
    public class RemoteFFmpegVersion
    {
        public string WebLink { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Version Version { get; set; }
    }
}
