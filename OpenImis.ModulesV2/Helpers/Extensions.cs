using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OpenImis.ModulesV2.Helpers
{
    public static class Extensions
    {
        public static string XMLSerialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlserializer.Serialize(writer, value, emptyNs);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
