using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class CrawlConfiguration : EntityExportElement
    {
        private string _notes;

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_MaxRecursionDepth { get => MaxRecursionDepth.ToUInt16Xml(DbConstants.DbColDefaultValue_MaxRecursionDepth); set => MaxRecursionDepth = value.FromXmlUInt16(MaxRecursionDepth); }
        [XmlIgnore]
        public ushort MaxRecursionDepth { get; set; } = DbConstants.DbColDefaultValue_MaxRecursionDepth;

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_MaxTotalItems { get => MaxTotalItems.ToUInt64Xml(DbConstants.DbColDefaultValue_MaxTotalItems); set => MaxTotalItems = value.FromXmlUInt64(MaxTotalItems); }
        [XmlIgnore]
        public ulong MaxTotalItems { get; set; } = DbConstants.DbColDefaultValue_MaxTotalItems;

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_IsInactive { get => IsInactive.ToBooleanXml(false); set => IsInactive = value.FromXmlBoolean(IsInactive); }
        [XmlIgnore]
        public bool IsInactive { get; set; }

        [XmlText]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }
    }
}
