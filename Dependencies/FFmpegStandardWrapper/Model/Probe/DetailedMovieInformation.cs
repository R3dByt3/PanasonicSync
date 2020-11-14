using Newtonsoft.Json;
using System.Collections.Generic;

namespace FFmpegStandardWrapper.Model.Probe
{
    public class DetailedMovieInformation
    {
        [JsonProperty("streams")]
        public List<Stream> Streams { get; set; }

        [JsonProperty("format")]
        public Format Format { get; set; }
    }
}
