using Newtonsoft.Json;

namespace FFmpegStandardWrapper.Model.Probe
{
    public class FormatTags
    {
        [JsonProperty("major_brand")]
        public string MajorBrand { get; set; }

        [JsonProperty("minor_version")]
        public long MinorVersion { get; set; }

        [JsonProperty("compatible_brands")]
        public string CompatibleBrands { get; set; }

        [JsonProperty("encoder")]
        public string Encoder { get; set; }
    }
}
