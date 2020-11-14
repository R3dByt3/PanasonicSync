using Newtonsoft.Json;
using System;

namespace FFmpegStandardWrapper.Converter
{
    public class TimeSpanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(string))
                return true;

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var input = reader.Value as string;

            return TimeSpan.Parse(input.Split('.')[0]);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var timeSpan = (TimeSpan)value;

            writer.WriteValue(timeSpan);
        }
    }
}
