using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public abstract class ExportElement : ISynchonizedElement
    {
        protected internal object SyncRoot { get; } = new();

        object ISynchonizedElement.SyncRoot => SyncRoot;

        [XmlAttribute(nameof(CreatedOn))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_CreatedOn { get => CreatedOn.ToDateTimeXml(); set => CreatedOn = value.FromXmlDateTime(CreatedOn); }
        [XmlIgnore]
        public DateTime CreatedOn { get; set; }

        [XmlAttribute(nameof(ModifiedOn))]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string __XML_ModifiedOn { get => ModifiedOn.ToDateTimeXml(); set => ModifiedOn = value.FromXmlDateTime(ModifiedOn); }
        [XmlIgnore]
        public DateTime ModifiedOn { get; set; }
    }
}
