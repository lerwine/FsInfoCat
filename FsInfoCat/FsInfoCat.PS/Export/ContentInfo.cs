using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class ContentInfo : ExportSet.ContentInfoBase
    {
        private readonly RedundantSetBase.Collection _redundantSets;

        public IEnumerable<File> GetFiles()
        {
            ExportSet exportSet = ExportSet;
            if (exportSet is null)
                return null;
            Guid id = Id;
            return exportSet.FileSystems.SelectMany(fs => fs.Volumes.Select(v => v.RootDirectory).Where(r => r is not null)).SelectMany(d => d.GetAllFiles())
                .Where(f => f.ContentId == id);
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

        public ContentInfo()
        {
            _redundantSets = new RedundantSetBase.Collection(this);
        }

        public abstract class RedundantSetBase : EntityExportElement, IOwnedElement<ContentInfo>
        {
            [XmlIgnore]
            public ContentInfo ContentInfo { get; private set; }

            ContentInfo IOwnedElement<ContentInfo>.Owner => ContentInfo;

            internal class Collection : OwnedCollection<ContentInfo, RedundantSet>
            {
                internal Collection(ContentInfo owner) : base(owner) { }

                internal Collection(ContentInfo owner, IEnumerable<RedundantSet> items) : base(owner, items) { }

                protected override void OnItemAdding(RedundantSet item) => item.ContentInfo = Owner;

                protected override void OnItemRemoved(RedundantSet item) => item.ContentInfo = null;
            }
        }
    }
}
