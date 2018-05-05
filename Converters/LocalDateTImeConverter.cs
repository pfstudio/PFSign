using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PFSign.Converters
{
    /// <summary>
    /// 将时间转换为当地时间
    /// </summary>
    public class LocalDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"));
        }
    }
}
