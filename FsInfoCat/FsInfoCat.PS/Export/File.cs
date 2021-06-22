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

        [XmlAttribute(nameof(BinaryPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_BinaryPropertySetId { get => BinaryPropertySetId.ToGuidXml(); set => BinaryPropertySetId = value.FromXmlGuid(BinaryPropertySetId); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public Guid BinaryPropertySetId { get; set; }
        public BinaryPropertySet GetBinaryProperties()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = BinaryPropertySetId;
            return exportSet.BinaryPropertySets.FirstOrDefault(e => e.Id == id);
        }

        [Obsolete]
        public Guid? ExtendedPropertyId { get; set; }

        [XmlAttribute(nameof(SummaryPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_SummaryPropertySetId { get => SummaryPropertySetId.ToGuidXml(); set => SummaryPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? SummaryPropertySetId { get; set; }

        public SummaryPropertySet GetSummaryProperties()
        {
            Guid? id = SummaryPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.SummaryPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(DocumentPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DocumentPropertySetId { get => DocumentPropertySetId.ToGuidXml(); set => DocumentPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? DocumentPropertySetId { get; set; }

        public DocumentPropertySet GetDocumentProperties()
        {
            Guid? id = DocumentPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.DocumentPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(AudioPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_AudioPropertySetId { get => AudioPropertySetId.ToGuidXml(); set => AudioPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? AudioPropertySetId { get; set; }

        public AudioPropertySet GetAudioProperties()
        {
            Guid? id = AudioPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.AudioPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(DRMPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DRMPropertySetId { get => DRMPropertySetId.ToGuidXml(); set => DRMPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? DRMPropertySetId { get; set; }

        public DRMPropertySet GetDRMProperties()
        {
            Guid? id = DRMPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.DRMPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(GPSPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_GPSPropertySetId { get => GPSPropertySetId.ToGuidXml(); set => GPSPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? GPSPropertySetId { get; set; }

        public GPSPropertySet GetGPSProperties()
        {
            Guid? id = GPSPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.GPSPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(ImagePropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ImagePropertySetId { get => ImagePropertySetId.ToGuidXml(); set => ImagePropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? ImagePropertySetId { get; set; }

        public ImagePropertySet GetImageProperties()
        {
            Guid? id = ImagePropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.ImagePropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(MediaPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_MediaPropertySetId { get => MediaPropertySetId.ToGuidXml(); set => MediaPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? MediaPropertySetId { get; set; }

        public MediaPropertySet GetMediaProperties()
        {
            Guid? id = MediaPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.MediaPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(MusicPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_MusicPropertySetId { get => MusicPropertySetId.ToGuidXml(); set => MusicPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? MusicPropertySetId { get; set; }

        public MusicPropertySet GetMusicProperties()
        {
            Guid? id = MusicPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.MusicPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(PhotoPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_PhotoPropertySetId { get => PhotoPropertySetId.ToGuidXml(); set => PhotoPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? PhotoPropertySetId { get; set; }

        public PhotoPropertySet GetPhotoProperties()
        {
            Guid? id = PhotoPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.PhotoPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(RecordedTVPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_RecordedTVPropertySetId { get => RecordedTVPropertySetId.ToGuidXml(); set => RecordedTVPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? RecordedTVPropertySetId { get; set; }

        public RecordedTVPropertySet GetRecordedTVProperties()
        {
            Guid? id = RecordedTVPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.RecordedTVPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(VideoPropertySetId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_VideoPropertySetId { get => VideoPropertySetId.ToGuidXml(); set => VideoPropertySetId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? VideoPropertySetId { get; set; }

        public VideoPropertySet GetVideoProperties()
        {
            Guid? id = VideoPropertySetId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.VideoPropertySets.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        public Redundancy GetRedundancy()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = Id;
            return exportSet.BinaryPropertySets.SelectMany(c => c.RedundantSets.SelectMany(r => r.Redundancies)).FirstOrDefault(r => r.FileId == id);
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

        internal bool IsProcessed { get; set; }

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
