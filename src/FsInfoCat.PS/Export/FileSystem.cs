using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class FileSystem : ExportSet.FileSystemBase
    {
        private string _notes;

        [XmlAttribute(nameof(CaseSensitiveSearch))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_CaseSensitiveSearch { get => CaseSensitiveSearch.ToBooleanXml(false); set => CaseSensitiveSearch = value.FromXmlBoolean(CaseSensitiveSearch); }
#pragma warning restore IDE1006 // Naming Styles

        internal void SetAllProcessedFlags(bool value)
        {
            foreach (Volume volume in Volumes)
                volume.SetAllProcessedFlags(value);
        }

        [XmlIgnore]
        public bool CaseSensitiveSearch { get; set; }

        [XmlAttribute(nameof(ReadOnly))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ReadOnly { get => ReadOnly.ToBooleanXml(false); set => ReadOnly = value.FromXmlBoolean(ReadOnly); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool ReadOnly { get; set; }

        [XmlAttribute(nameof(MaxNameLength))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Priority { get => MaxNameLength.ToUInt32Xml(DbConstants.DbColDefaultValue_MaxNameLength); set => MaxNameLength = value.FromXmlUInt32(MaxNameLength); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public uint MaxNameLength { get; set; }

        [XmlAttribute(nameof(DefaultDriveType))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DefaultDriveType { get => DefaultDriveType.ToDriveTypeXml(); set => DefaultDriveType = value.FromXmlDriveType(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DriveType? DefaultDriveType { get; set; }

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_IsInactive { get => IsInactive.ToBooleanXml(false); set => IsInactive = value.FromXmlBoolean(IsInactive); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool IsInactive { get; set; }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        [XmlElement(nameof(Volume))]
        public Collection<Volume> Volumes { get; set; }

        public abstract class VolumeBase : EntityExportElement
        {
            private string _displayName = "";
            private string _volumeName = "";

            [XmlAttribute]
            public string DisplayName { get => _displayName; set => _displayName = value.TrimmedNotNull(); }

            [XmlAttribute]
            public string VolumeName { get => _volumeName; set => _volumeName = value.TrimmedNotNull(); }

            [XmlAttribute]
            public VolumeIdentifier Identifier { get; set; }

            [XmlIgnore]
            public FileSystem FileSystem { get; private set; }
        }

        public abstract class SymbolicNameBase : EntityExportElement
        {
            private string _name = "";

            [XmlAttribute]
            public string Name { get => _name; set => _name = value.TrimmedNotNull(); }

            [XmlIgnore]
            public FileSystem FileSystem { get; private set; }
        }
    }
}
