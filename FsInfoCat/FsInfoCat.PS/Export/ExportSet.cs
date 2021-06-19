using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class ExportSet
    {
        [XmlElement(nameof(FileSystem))]
        public Collection<FileSystem> FileSystems { get; set; }

        [XmlElement(nameof(ExtendedProperties))]
        public Collection<ExtendedProperties> ExtendedProperties { get; set; }

        [XmlElement(nameof(ContentInfo))]
        public Collection<ContentInfo> ContentInfos { get; set; }

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
