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
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_CreatedOn { get => CreatedOn.ToDateTimeXml(); set => CreatedOn = value.FromXmlDateTime(CreatedOn); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime CreatedOn { get; set; }

        [XmlAttribute(nameof(ModifiedOn))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ModifiedOn { get => ModifiedOn.ToDateTimeXml(); set => ModifiedOn = value.FromXmlDateTime(ModifiedOn); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime ModifiedOn { get; set; }
    }
}
