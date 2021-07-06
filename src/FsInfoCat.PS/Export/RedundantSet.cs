using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class RedundantSet : BinaryPropertySet.RedundantSetBase
    {
        private string _reference = "";
        private string _notes;
        private readonly RedundancyBase.Collection _redundancies;

        [XmlAttribute]
        public string Reference { get => _reference; set => _reference = value.TrimmedNotNull(); }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        [XmlElement(nameof(Redundancy))]
        public Collection<Redundancy> Redundancies
        {
            get => _redundancies;
            set
            {
                if (ReferenceEquals(_redundancies, value))
                    return;
                if (value is RedundancyBase.Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _redundancies.Clear();
                    _redundancies.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public RedundantSet()
        {
            _redundancies = new RedundancyBase.Collection(this);
        }

        public abstract class RedundancyBase : ExportElement, IOwnedElement<RedundantSet>
        {
            [XmlAttribute(nameof(FileId))]
            [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
            public string __XML_FileId { get => FileId.ToGuidXml(); set => FileId = value.FromXmlGuid(FileId); }
#pragma warning restore IDE1006 // Naming Styles
            [XmlIgnore]
            public Guid FileId { get; set; }

            [XmlIgnore]
            public RedundantSet RedundantSet { get; private set; }

            RedundantSet IOwnedElement<RedundantSet>.Owner => RedundantSet;

            internal class Collection : OwnedCollection<RedundantSet, Redundancy>
            {
                internal Collection(RedundantSet owner) : base(owner) { }

                internal Collection(RedundantSet owner, IEnumerable<Redundancy> items) : base(owner, items) { }

                protected override void OnItemAdding(Redundancy item) => item.RedundantSet = Owner;

                protected override void OnItemRemoved(Redundancy item) => item.RedundantSet = null;
            }
        }
    }
}
