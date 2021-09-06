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
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_MaxRecursionDepth { get => MaxRecursionDepth.ToUInt16Xml(DbConstants.DbColDefaultValue_MaxRecursionDepth); set => MaxRecursionDepth = value.FromXmlUInt16(MaxRecursionDepth); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public ushort MaxRecursionDepth { get; set; } = DbConstants.DbColDefaultValue_MaxRecursionDepth;

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_MaxTotalItems { get => MaxTotalItems.HasValue ? null : MaxTotalItems.ToUInt64Xml(); set => MaxTotalItems = value.FromXmlUInt64(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public ulong? MaxTotalItems { get; set; }

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_IsInactive { get => IsInactive.ToBooleanXml(false); set => IsInactive = value.FromXmlBoolean(IsInactive); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool IsInactive { get; set; }

        [XmlText]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }
    }
}
