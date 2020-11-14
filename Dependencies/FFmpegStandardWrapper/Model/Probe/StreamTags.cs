using Newtonsoft.Json;

namespace FFmpegStandardWrapper.Model.Probe
{
    public class StreamTags
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("handler_name")]
        public string HandlerName { get; set; }
    }
}
