using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class SymbolicName : FileSystem.SymbolicNameBase
    {
        private string _notes;

        [XmlAttribute(nameof(Priority))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_Priority { get => Priority.ToInt32Xml(0); set => Priority = value.FromXmlInt32(Priority); }
        [XmlIgnore]
        public int Priority { get; set; }

        [XmlAttribute(nameof(IsInactive))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_IsInactive { get => IsInactive.ToBooleanXml(false); set => IsInactive = value.FromXmlBoolean(IsInactive); }
        [XmlIgnore]
        public bool IsInactive { get; set; }

        [XmlText]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }
    }
}
