using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class BinaryProperties : ExportSet.BinaryPropertiesBase
    {
        private readonly RedundantSetBase.Collection _redundantSets;

        public IEnumerable<File> GetFiles()
        {
            ExportSet exportSet = ExportSet;
            if (exportSet is null)
                return null;
            Guid id = Id;
            return exportSet.FileSystems.SelectMany(fs => fs.Volumes.Select(v => v.RootDirectory).Where(r => r is not null)).SelectMany(d => d.GetAllFiles())
                .Where(f => f.BinaryPropertiesId == id);
        }

        [XmlElement(nameof(RedundantSet))]
        public Collection<RedundantSet> RedundantSets
        {
            get => _redundantSets;
            set
            {
                if (ReferenceEquals(_redundantSets, value))
                    return;
                if (value is RedundantSetBase.Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _redundantSets.Clear();
                    _redundantSets.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public BinaryProperties()
        {
            _redundantSets = new RedundantSetBase.Collection(this);
        }

        public abstract class RedundantSetBase : EntityExportElement, IOwnedElement<BinaryProperties>
        {
            [XmlIgnore]
            public BinaryProperties BinaryProperties { get; private set; }

            BinaryProperties IOwnedElement<BinaryProperties>.Owner => BinaryProperties;

            internal class Collection : OwnedCollection<BinaryProperties, RedundantSet>
            {
                internal Collection(BinaryProperties owner) : base(owner) { }

                internal Collection(BinaryProperties owner, IEnumerable<RedundantSet> items) : base(owner, items) { }

                protected override void OnItemAdding(RedundantSet item) => item.BinaryProperties = Owner;

                protected override void OnItemRemoved(RedundantSet item) => item.BinaryProperties = null;
            }
        }
    }
}
