using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace OpenImis.ModulesV3.Utils
{
    public static class DateTimeFormats
    {
        public static CultureInfo cultureInfo = CultureInfo.InvariantCulture;
        public const string IsoDateFormat = "yyyy-MM-dd";
        public const string IsoDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        public const string FileNameDateTimeFormat = "yyyy-MM-ddTHH-mm-ss";

    }

    public class CustomFormatDatetimeSerializer : JsonConverter<DateTime>
    {
        protected string format;

        public CustomFormatDatetimeSerializer(string format)
        {
            this.format = format;
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateTime.ParseExact(reader.Value.ToString(), this.format, DateTimeFormats.cultureInfo);
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(this.format, DateTimeFormats.cultureInfo));
        }
    }

    public class IsoDateOnlyDatetimeSerializer : CustomFormatDatetimeSerializer
    {
        public IsoDateOnlyDatetimeSerializer() : base(DateTimeFormats.IsoDateFormat)
        {
        }
    }

    public class ImisSPXmlWriter : XmlTextWriter
    {
        public ImisSPXmlWriter(TextWriter writer) : base(writer) {
        }
        public ImisSPXmlWriter(Stream stream, Encoding encoding) : base(stream, encoding) { }
        public ImisSPXmlWriter(string filename, Encoding encoding) : base(filename, encoding) { }

        public override void WriteRaw(string data)
        {
            DateTime dt;

            if (DateTime.TryParse(data, out dt))
                base.WriteRaw(dt.ToString(DateTimeFormats.IsoDateOnlyFormat, DateTimeFormats.cultureInfo));
            else
                base.WriteRaw(data);
        }

        public override void WriteStartDocument()
        {
        }
    }
}
