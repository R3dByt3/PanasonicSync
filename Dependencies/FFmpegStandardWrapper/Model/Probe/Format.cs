﻿using FFmpegStandardWrapper.Converter;
using Newtonsoft.Json;
using System;

namespace FFmpegStandardWrapper.Model.Probe
{
    public class Format
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("nb_streams")]
        public long NbStreams { get; set; }

        [JsonProperty("nb_programs")]
        public long NbPrograms { get; set; }

        [JsonProperty("format_name")]
        public string FormatName { get; set; }

        [JsonProperty("format_long_name")]
        public string FormatLongName { get; set; }

        [JsonProperty("start_time")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan StartTime { get; set; }

        [JsonProperty("duration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("bit_rate")]
        public long BitRate { get; set; }

        [JsonProperty("probe_score")]
        public long ProbeScore { get; set; }

        [JsonProperty("tags")]
        public FormatTags Tags { get; set; }
    }
}
