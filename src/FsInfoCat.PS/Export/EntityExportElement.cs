using System;
using System.ComponentModel;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public abstract class EntityExportElement : ExportElement
    {
        private Guid? _id;

        [XmlAttribute(nameof(Id))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Id { get => Id.ToGuidXml(); set => _id = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public Guid Id
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (!_id.HasValue)
                        _id = Guid.NewGuid();
                }
                finally { Monitor.Exit(SyncRoot); }
                return _id.Value;
            }
            set => _id = value;
        }
    }
}
