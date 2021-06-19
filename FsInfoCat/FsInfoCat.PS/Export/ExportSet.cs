using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public sealed class ExportSet
    {
        [XmlElement(nameof(FileSystem))]
        public Collection<FileSystem> FileSystems { get; set; }

        [XmlElement(nameof(ExtendedProperties))]
        public Collection<ExtendedProperties> ExtendedProperties { get; set; }

        [XmlElement(nameof(ContentInfo))]
        public Collection<ContentInfo> ContentInfos { get; set; }

        private static XmlWriterSettings GetDefaultXmlWriterSettings() => new()
        {
            Indent = true,
            Encoding = new UTF8Encoding(false, true)
        };

        public static ExportSet Load(string path)
        {
            using XmlReader xmlReader = XmlReader.Create(path);
            return Load(xmlReader);
        }

        public static ExportSet Load(TextReader input)
        {
            using XmlReader xmlReader = XmlReader.Create(input);
            return Load(xmlReader);
        }

        public static ExportSet LoadXml(string xml)
        {
            using StringReader reader = new(xml);
            return Load(reader);
        }

        public static ExportSet Load(Stream input)
        {
            using XmlReader xmlReader = XmlReader.Create(input);
            return Load(xmlReader);
        }

        public static ExportSet Load(XmlReader xmlReader)
        {
            XmlSerializer serializer = new(typeof(ExportSet));
            return (ExportSet)serializer.Deserialize(xmlReader);
        }

        public void WriteTo(string path, XmlWriterSettings settings = null)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(path, settings ?? GetDefaultXmlWriterSettings());
            WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        public void WriteTo(Stream output, XmlWriterSettings settings = null)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(output, settings ?? GetDefaultXmlWriterSettings());
            WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        public void WriteTo(TextWriter output, XmlWriterSettings settings = null)
        {
            using XmlWriter xmlWriter = XmlWriter.Create(output, settings ?? GetDefaultXmlWriterSettings());
            WriteTo(xmlWriter);
            xmlWriter.Flush();
        }

        public string ToXmlString(XmlWriterSettings settings = null)
        {
            StringWriter writer = new();
            WriteTo(writer, settings);
            return writer.ToString();
        }

        public void WriteTo(XmlWriter xmlWriter)
        {
            XmlSerializer serializer = new(typeof(ExportSet));
            serializer.Serialize(xmlWriter, this);
        }

        public abstract class FileSystemBase : EntityExportElement
        {
            private string _displayName = "";

            [XmlAttribute]
            public string DisplayName { get => _displayName; set => _displayName = value.TrimmedNotNull(); }

            [XmlElement(nameof(SymbolicName))]
            public Collection<SymbolicName> SymbolicNames { get; set; }

            [XmlIgnore]
            public ExportSet ExportSet { get; private set; }
        }

        public abstract class ExtendedPropertiesBase : EntityExportElement
        {
            [XmlIgnore]
            public ExportSet ExportSet { get; private set; }
        }

        public abstract class ContentInfoBase : EntityExportElement
        {
            [XmlAttribute]
            public long Length { get; set; }

            [XmlAttribute]
            public MD5Hash? Hash { get; set; }

            [XmlIgnore]
            public ExportSet ExportSet { get; private set; }
        }
    }
}
