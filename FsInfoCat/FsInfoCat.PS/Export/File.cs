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

        [XmlAttribute(nameof(BinaryPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_BinaryPropertiesId { get => BinaryPropertiesId.ToGuidXml(); set => BinaryPropertiesId = value.FromXmlGuid(BinaryPropertiesId); }
#pragma warning restore IDE1006 // Naming Styles
        [XmlIgnore]
        public Guid BinaryPropertiesId { get; set; }
        public BinaryProperties GetBinaryProperties()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = BinaryPropertiesId;
            return exportSet.BinaryProperties.FirstOrDefault(e => e.Id == id);
        }

        [Obsolete]
        public Guid? ExtendedPropertyId { get; set; }

        [XmlAttribute(nameof(SummaryPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_SummaryPropertiesId { get => SummaryPropertiesId.ToGuidXml(); set => SummaryPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? SummaryPropertiesId { get; set; }

        public SummaryProperties GetSummaryProperties()
        {
            Guid? id = SummaryPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.SummaryProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(DocumentPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DocumentPropertiesId { get => DocumentPropertiesId.ToGuidXml(); set => DocumentPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? DocumentPropertiesId { get; set; }

        public DocumentProperties GetDocumentProperties()
        {
            Guid? id = DocumentPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.DocumentProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(AudioPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_AudioPropertiesId { get => AudioPropertiesId.ToGuidXml(); set => AudioPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? AudioPropertiesId { get; set; }

        public AudioProperties GetAudioProperties()
        {
            Guid? id = AudioPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.AudioProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(DRMPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_DRMPropertiesId { get => DRMPropertiesId.ToGuidXml(); set => DRMPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? DRMPropertiesId { get; set; }

        public DRMProperties GetDRMProperties()
        {
            Guid? id = DRMPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.DRMProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(GPSPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_GPSPropertiesId { get => GPSPropertiesId.ToGuidXml(); set => GPSPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? GPSPropertiesId { get; set; }

        public GPSProperties GetGPSProperties()
        {
            Guid? id = GPSPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.GPSProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(ImagePropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_ImagePropertiesId { get => ImagePropertiesId.ToGuidXml(); set => ImagePropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? ImagePropertiesId { get; set; }

        public ImageProperties GetImageProperties()
        {
            Guid? id = ImagePropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.ImageProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(MediaPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_MediaPropertiesId { get => MediaPropertiesId.ToGuidXml(); set => MediaPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? MediaPropertiesId { get; set; }

        public MediaProperties GetMediaProperties()
        {
            Guid? id = MediaPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.MediaProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(MusicPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_MusicPropertiesId { get => MusicPropertiesId.ToGuidXml(); set => MusicPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? MusicPropertiesId { get; set; }

        public MusicProperties GetMusicProperties()
        {
            Guid? id = MusicPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.MusicProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(PhotoPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_PhotoPropertiesId { get => PhotoPropertiesId.ToGuidXml(); set => PhotoPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? PhotoPropertiesId { get; set; }

        public PhotoProperties GetPhotoProperties()
        {
            Guid? id = PhotoPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.PhotoProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(RecordedTVPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_RecordedTVPropertiesId { get => RecordedTVPropertiesId.ToGuidXml(); set => RecordedTVPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? RecordedTVPropertiesId { get; set; }

        public RecordedTVProperties GetRecordedTVProperties()
        {
            Guid? id = RecordedTVPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.RecordedTVProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlAttribute(nameof(VideoPropertiesId))]
        [EditorBrowsable(EditorBrowsableState.Never)]
#pragma warning disable IDE1006 // Naming Styles
        public string __XML_VideoPropertiesId { get => VideoPropertiesId.ToGuidXml(); set => VideoPropertiesId = value.FromXmlGuid(); }
#pragma warning restore IDE1006 // Naming Styles

        [XmlIgnore]
        public Guid? VideoPropertiesId { get; set; }

        public VideoProperties GetVideoProperties()
        {
            Guid? id = VideoPropertiesId;
            if (!id.HasValue)
                return null;
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            return exportSet.VideoProperties.FirstOrDefault(e => e.Id == id.Value);
        }

        [XmlElement]
        public string Notes { get => _notes; set => _notes = value.NullIfWhitespace(); }

        public Redundancy GetRedundancy()
        {
            ExportSet exportSet = Parent?.Volume?.FileSystem?.ExportSet;
            if (exportSet is null)
                return null;
            Guid id = Id;
            return exportSet.BinaryProperties.SelectMany(c => c.RedundantSets.SelectMany(r => r.Redundancies)).FirstOrDefault(r => r.FileId == id);
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
