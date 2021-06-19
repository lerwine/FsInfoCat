using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace FsInfoCat.PS.Export
{
    public class File : Subdirectory.FileBase
    {
        private string _notes;
        private readonly ComparisonBase.Collection _comparisonSources;
        private readonly AccessError.Collection _accessErrors;

        [XmlAttribute(nameof(Options))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Options { get => Options.ToFileCrawlOptionsXml(); set => Options = value.FromXmlFileCrawlOptions(Options); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public FileCrawlOptions Options { get; set; }

        [XmlAttribute(nameof(LastHashCalculation))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_LastHashCalculation { get => LastHashCalculation.ToDateTimeXml(); set => LastHashCalculation = value.FromXmlDateTime(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime? LastHashCalculation { get; set; }

        [XmlAttribute(nameof(Deleted))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_Deleted { get => Deleted.ToBooleanXml(false); set => Deleted = value.FromXmlBoolean(Deleted); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public bool Deleted { get; set; }

        [XmlAttribute(nameof(LastAccessed))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_LastAccessed { get => LastAccessed.ToDateTimeXml(); set => LastAccessed = value.FromXmlDateTime(LastAccessed); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime LastAccessed { get; set; }

        [XmlAttribute(nameof(CreationTime))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_CreationTime { get => CreationTime.ToDateTimeXml(); set => CreationTime = value.FromXmlDateTime(CreationTime); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime CreationTime { get; set; }

        [XmlAttribute(nameof(LastWriteTime))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_LastWriteTime { get => LastWriteTime.ToDateTimeXml(); set => LastWriteTime = value.FromXmlDateTime(LastWriteTime); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public DateTime LastWriteTime { get; set; }

        [XmlAttribute(nameof(ContentId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ContentId { get => ContentId.ToGuidXml(); set => ContentId = value.FromXmlGuid(ContentId); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public Guid ContentId { get; set; }
        public ContentInfo GetContent()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = ContentId;
            return exportSet.ContentInfos.FirstOrDefault(e => e.Id == id);
        }

        [XmlAttribute(nameof(ExtendedPropertyId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ExtendedPropertyId { get => ExtendedPropertyId.ToGuidXml(); set => ExtendedPropertyId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public Guid? ExtendedPropertyId { get; set; }
        public ExtendedProperties GetExtendedProperties()
        {
            Guid? id = ExtendedPropertyId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.ExtendedProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        public Redundancy GetRedundancy()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = Id;
            return exportSet.ContentInfos.SelectMany(c => c.RedundantSets.SelectMany(r => r.Redundancies)).FirstOrDefault(r => r.FileId == id);
        }

        [XmlElement(nameof(AccessError))]
        public Collection<AccessError> AccessErrors
        {
            get => _accessErrors;
            set
            {
                if (ReferenceEquals(_accessErrors, value))
                    return;
                if (value is AccessError.Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _accessErrors.Clear();
                    _accessErrors.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [XmlElement(nameof(Comparison))]
        public Collection<Comparison> ComparisonSources
        {
            get => _comparisonSources;
            set
            {
                if (ReferenceEquals(_comparisonSources, value))
                    return;
                if (value is ComparisonBase.Collection)
                    throw new InvalidOperationException();
                Monitor.Enter(SyncRoot);
                try
                {
                    _comparisonSources.Clear();
                    _comparisonSources.AddRange(value);
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public IEnumerable<Comparison> GetComparisonTargets()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return Enumerable.Empty<Comparison>();
            Guid id = Id;
            return exportSet.FileSystems.SelectMany(fs => fs.Volumes.Select(v => v.RootDirectory).Where(d => d is not null))
                .SelectMany(d => d.GetAllFiles()).SelectMany(f => f.ComparisonSources).Where(c => c.TargetFileId == id);
        }

        public File()
        {
            _comparisonSources = new ComparisonBase.Collection(this);
            _accessErrors = new AccessError.Collection(this);
        }

        public abstract class ComparisonBase : ExportElement, IOwnedElement<File>
        {
            [XmlIgnore]
            public File SourceFile { get; private set; }

            File IOwnedElement<File>.Owner => SourceFile;

            internal class Collection : OwnedCollection<File, Comparison>
            {
                internal Collection(File owner) : base(owner) { }

                internal Collection(File owner, IEnumerable<Comparison> items) : base(owner, items) { }

                protected override void OnItemAdding(Comparison item) => item.SourceFile = Owner;

                protected override void OnItemRemoved(Comparison item) => item.SourceFile = null;
            }
        }

        public class AccessError : AccessErrorElement<File>, IOwnedElement<File>
        {
            private File _target;

            public override File Target => _target;

            File IOwnedElement<File>.Owner => _target;

            internal class Collection : OwnedCollection<File, AccessError>
            {
                internal Collection(File owner) : base(owner) { }

                internal Collection(File owner, IEnumerable<AccessError> items) : base(owner, items) { }

                protected override void OnItemAdding(AccessError item) => item._target = Owner;

                protected override void OnItemRemoved(AccessError item) => item._target = null;
            }
        }
    }
}
