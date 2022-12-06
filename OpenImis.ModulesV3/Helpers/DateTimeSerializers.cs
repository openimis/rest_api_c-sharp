using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace OpenImis.ModulesV3.Helpers
{
    public class CustomFormatDatetimeSerializer : JsonConverter<DateTime?>
    {
        protected string format;
        protected string errorMessage;

        public CustomFormatDatetimeSerializer(string format)
        {
            this.format = format;
            this.errorMessage = "Invalid datetime field";
        }

        public CustomFormatDatetimeSerializer(string format, string errorMessage)
        {
            this.format = format;
            this.errorMessage = errorMessage;
        }

        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (DateTime.TryParseExact(reader.Value.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }
            else
            {
                throw new BusinessException(errorMessage);
            }
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            DateTime dt = value ?? new DateTime();
            writer.WriteValue(dt.ToString(format, CultureInfo.InvariantCulture));

        }
    }

    public class IsoDateSerializer : CustomFormatDatetimeSerializer
    {
        public IsoDateSerializer() : base(DateTimeFormats.IsoDateFormat)
        {
        }

        public IsoDateSerializer(string errorMessage) : base(DateTimeFormats.IsoDateFormat, errorMessage)
        {
        }
    }

    public class IsoDateTimeSerializer : CustomFormatDatetimeSerializer
    {
        public IsoDateTimeSerializer() : base(DateTimeFormats.IsoDateTimeFormat)
        {
        }

        public IsoDateTimeSerializer(string errorMessage) : base(DateTimeFormats.IsoDateTimeFormat, errorMessage)
        {
        }
    }

    public class ImisSPXmlWriter : XmlTextWriter
    {
        public ImisSPXmlWriter(TextWriter writer) : base(writer)
        {
        }
        public ImisSPXmlWriter(Stream stream, Encoding encoding) : base(stream, encoding) { }
        public ImisSPXmlWriter(string filename, Encoding encoding) : base(filename, encoding) { }

        public override void WriteRaw(string data)
        {
            DateTime dt;

            if (DateTime.TryParse(data, out dt))
                base.WriteRaw(dt.ToString(DateTimeFormats.IsoDateFormat, CultureInfo.InvariantCulture));
            else
                base.WriteRaw(data);
        }

        public override void WriteStartDocument()
        {
        }
    }
}
