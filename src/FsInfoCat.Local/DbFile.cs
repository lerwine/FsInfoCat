using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class DbFile : LocalDbEntity, ILocalFile
    {
        #region Fields

        public const string TABLE_NAME = "Files";

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<FileCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<DateTime?> _lastHashCalculation;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _deleted;
        private readonly IPropertyChangeTracker<DateTime> _creationTime;
        private readonly IPropertyChangeTracker<DateTime> _lastWriteTime;
        private readonly IPropertyChangeTracker<Guid> _parentId;
        private readonly IPropertyChangeTracker<Guid> _binaryPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _summaryPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _documentPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _audioPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _drmPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _gpsPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _imagePropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _mediaPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _musicPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _photoPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _recordedTVPropertySetId;
        private readonly IPropertyChangeTracker<Guid?> _videoPropertySetId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<BinaryPropertySet> _binaryPropertySet;
        private readonly IPropertyChangeTracker<Redundancy> _redundancy;
        private readonly IPropertyChangeTracker<SummaryPropertySet> _summaryPropertySet;
        private readonly IPropertyChangeTracker<DocumentPropertySet> _documentProperties;
        private readonly IPropertyChangeTracker<AudioPropertySet> _audioProperties;
        private readonly IPropertyChangeTracker<DRMPropertySet> _drmProperties;
        private readonly IPropertyChangeTracker<GPSPropertySet> _gpsProperties;
        private readonly IPropertyChangeTracker<ImagePropertySet> _imageProperties;
        private readonly IPropertyChangeTracker<MediaPropertySet> _mediaProperties;
        private readonly IPropertyChangeTracker<MusicPropertySet> _musicProperties;
        private readonly IPropertyChangeTracker<PhotoPropertySet> _photoProperties;
        private readonly IPropertyChangeTracker<RecordedTVPropertySet> _recordedTVProperties;
        private readonly IPropertyChangeTracker<VideoPropertySet> _videoProperties;
        private HashSet<FileAccessError> _accessErrors = new();
        private HashSet<FileComparison> _comparisonSources = new();
        private HashSet<FileComparison> _comparisonTargets = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual FileCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        public void SetDeleted(LocalDbContext dbContext)
        {
            throw new NotImplementedException();
        }

        public virtual DateTime? LastHashCalculation { get => _lastHashCalculation.GetValue(); set => _lastHashCalculation.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool Deleted { get => _deleted.GetValue(); set => _deleted.SetValue(value); }

        public DateTime CreationTime { get => _creationTime.GetValue(); set => _creationTime.SetValue(value); }

        public DateTime LastWriteTime { get => _lastWriteTime.GetValue(); set => _lastWriteTime.SetValue(value); }

        public virtual Guid ParentId
        {
            get => _parentId.GetValue();
            set
            {
                if (_parentId.SetValue(value))
                {
                    Subdirectory nav = _parent.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _parent.SetValue(null);
                }
            }
        }

        public virtual Guid BinaryPropertySetId
        {
            get => _binaryPropertySetId.GetValue();
            set
            {
                if (_binaryPropertySetId.SetValue(value))
                {
                    BinaryPropertySet nav = _binaryPropertySet.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? SummaryPropertySetId
        {
            get => _summaryPropertySetId.GetValue();
            set
            {
                if (_summaryPropertySetId.SetValue(value))
                {
                    SummaryPropertySet nav = _summaryPropertySet.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? DocumentPropertySetId
        {
            get => _documentPropertySetId.GetValue();
            set
            {
                if (_documentPropertySetId.SetValue(value))
                {
                    DocumentPropertySet nav = _documentProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? AudioPropertySetId
        {
            get => _audioPropertySetId.GetValue();
            set
            {
                if (_audioPropertySetId.SetValue(value))
                {
                    AudioPropertySet nav = _audioProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? DRMPropertySetId
        {
            get => _drmPropertySetId.GetValue();
            set
            {
                if (_drmPropertySetId.SetValue(value))
                {
                    DRMPropertySet nav = _drmProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? GPSPropertySetId
        {
            get => _gpsPropertySetId.GetValue();
            set
            {
                if (_gpsPropertySetId.SetValue(value))
                {
                    GPSPropertySet nav = _gpsProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? ImagePropertySetId
        {
            get => _imagePropertySetId.GetValue();
            set
            {
                if (_imagePropertySetId.SetValue(value))
                {
                    ImagePropertySet nav = _imageProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? MediaPropertySetId
        {
            get => _mediaPropertySetId.GetValue();
            set
            {
                if (_mediaPropertySetId.SetValue(value))
                {
                    MediaPropertySet nav = _mediaProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? MusicPropertySetId
        {
            get => _musicPropertySetId.GetValue();
            set
            {
                if (_musicPropertySetId.SetValue(value))
                {
                    MusicPropertySet nav = _musicProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? PhotoPropertySetId
        {
            get => _photoPropertySetId.GetValue();
            set
            {
                if (_photoPropertySetId.SetValue(value))
                {
                    PhotoPropertySet nav = _photoProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? RecordedTVPropertySetId
        {
            get => _recordedTVPropertySetId.GetValue();
            set
            {
                if (_recordedTVPropertySetId.SetValue(value))
                {
                    RecordedTVPropertySet nav = _recordedTVProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }

        public virtual Guid? VideoPropertySetId
        {
            get => _videoPropertySetId.GetValue();
            set
            {
                if (_videoPropertySetId.SetValue(value))
                {
                    VideoPropertySet nav = _videoProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryPropertySet.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryPropertySet.SetValue(null);
                }
            }
        }


        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryPropertySet.GetValue();
            set
            {
                if (_binaryPropertySet.SetValue(value))
                {
                    if (value is null)
                        _binaryPropertySetId.SetValue(Guid.Empty);
                    else
                        _binaryPropertySetId.SetValue(value.Id);
                }
            }
        }

        public virtual Subdirectory Parent
        {
            get => _parent.GetValue();
            set
            {
                if (_parent.SetValue(value))
                {
                    if (value is null)
                        _parentId.SetValue(Guid.Empty);
                    else
                        _parentId.SetValue(value.Id);
                }
            }
        }

        public virtual Redundancy Redundancy { get => _redundancy.GetValue(); set => _redundancy.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual SummaryPropertySet SummaryProperties
        {
            get => _summaryPropertySet.GetValue();
            set
            {
                if (_summaryPropertySet.SetValue(value))
                {
                    if (value is null)
                        _summaryPropertySetId.SetValue(null);
                    else
                        _summaryPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DocumentPropertySet DocumentProperties
        {
            get => _documentProperties.GetValue();
            set
            {
                if (_documentProperties.SetValue(value))
                {
                    if (value is null)
                        _documentPropertySetId.SetValue(null);
                    else
                        _documentPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual AudioPropertySet AudioProperties
        {
            get => _audioProperties.GetValue();
            set
            {
                if (_audioProperties.SetValue(value))
                {
                    if (value is null)
                        _audioPropertySetId.SetValue(null);
                    else
                        _audioPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DRMPropertySet DRMProperties
        {
            get => _drmProperties.GetValue();
            set
            {
                if (_drmProperties.SetValue(value))
                {
                    if (value is null)
                        _drmPropertySetId.SetValue(null);
                    else
                        _drmPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual GPSPropertySet GPSProperties
        {
            get => _gpsProperties.GetValue();
            set
            {
                if (_gpsProperties.SetValue(value))
                {
                    if (value is null)
                        _gpsPropertySetId.SetValue(null);
                    else
                        _gpsPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ImagePropertySet ImageProperties
        {
            get => _imageProperties.GetValue();
            set
            {
                if (_imageProperties.SetValue(value))
                {
                    if (value is null)
                        _imagePropertySetId.SetValue(null);
                    else
                        _imagePropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MediaPropertySet MediaProperties
        {
            get => _mediaProperties.GetValue();
            set
            {
                if (_mediaProperties.SetValue(value))
                {
                    if (value is null)
                        _mediaPropertySetId.SetValue(null);
                    else
                        _mediaPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MusicPropertySet MusicProperties
        {
            get => _musicProperties.GetValue();
            set
            {
                if (_musicProperties.SetValue(value))
                {
                    if (value is null)
                        _musicPropertySetId.SetValue(null);
                    else
                        _musicPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual PhotoPropertySet PhotoProperties
        {
            get => _photoProperties.GetValue();
            set
            {
                if (_photoProperties.SetValue(value))
                {
                    if (value is null)
                        _photoPropertySetId.SetValue(null);
                    else
                        _photoPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RecordedTVPropertySet RecordedTVProperties
        {
            get => _recordedTVProperties.GetValue();
            set
            {
                if (_recordedTVProperties.SetValue(value))
                {
                    if (value is null)
                        _recordedTVPropertySetId.SetValue(null);
                    else
                        _recordedTVPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VideoPropertySet VideoProperties
        {
            get => _videoProperties.GetValue();
            set
            {
                if (_videoProperties.SetValue(value))
                {
                    if (value is null)
                        _videoPropertySetId.SetValue(null);
                    else
                        _videoPropertySetId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ComparisonSources), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> ComparisonSources
        {
            get => _comparisonSources;
            set => CheckHashSetChanged(_comparisonSources, value, h => _comparisonSources = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ComparisonTargets), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> ComparisonTargets
        {
            get => _comparisonTargets;
            set => CheckHashSetChanged(_comparisonTargets, value, h => _comparisonTargets = h);
        }

        #endregion

        #region Explicit Members

        ILocalBinaryPropertySet ILocalFile.BinaryProperties { get => BinaryProperties; set => BinaryProperties = (BinaryPropertySet)value; }

        IBinaryPropertySet IFile.BinaryProperties { get => BinaryProperties; set => BinaryProperties = (BinaryPropertySet)value; }

        ILocalSubdirectory ILocalFile.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ISubdirectory IDbFsItem.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        IEnumerable<ILocalComparison> ILocalFile.ComparisonSources => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonSources => ComparisonSources.Cast<IComparison>();

        IEnumerable<ILocalComparison> ILocalFile.ComparisonTargets => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonTargets => ComparisonSources.Cast<IComparison>();

        ILocalSummaryPropertySet ILocalFile.SummaryProperties { get => SummaryProperties; set => SummaryProperties = (SummaryPropertySet)value; }
        ILocalDocumentPropertySet ILocalFile.DocumentProperties { get => DocumentProperties; set => DocumentProperties = (DocumentPropertySet)value; }
        ILocalAudioPropertySet ILocalFile.AudioProperties { get => AudioProperties; set => AudioProperties = (AudioPropertySet)value; }
        ILocalDRMPropertySet ILocalFile.DRMProperties { get => DRMProperties; set => DRMProperties = (DRMPropertySet)value; }
        ILocalGPSPropertySet ILocalFile.GPSProperties { get => GPSProperties; set => GPSProperties = (GPSPropertySet)value; }
        ILocalImagePropertySet ILocalFile.ImageProperties { get => ImageProperties; set => ImageProperties = (ImagePropertySet)value; }
        ILocalMediaPropertySet ILocalFile.MediaProperties { get => MediaProperties; set => MediaProperties = (MediaPropertySet)value; }
        ILocalMusicPropertySet ILocalFile.MusicProperties { get => MusicProperties; set => MusicProperties = (MusicPropertySet)value; }
        ILocalPhotoPropertySet ILocalFile.PhotoProperties { get => PhotoProperties; set => PhotoProperties = (PhotoPropertySet)value; }
        ILocalRecordedTVPropertySet ILocalFile.RecordedTVProperties { get => RecordedTVProperties; set => RecordedTVProperties = (RecordedTVPropertySet)value; }
        ILocalVideoPropertySet ILocalFile.VideoProperties { get => VideoProperties; set => VideoProperties = (VideoPropertySet)value; }
        ISummaryPropertySet IFile.SummaryProperties { get => SummaryProperties; set => SummaryProperties = (SummaryPropertySet)value; }
        IDocumentPropertySet IFile.DocumentProperties { get => DocumentProperties; set => DocumentProperties = (DocumentPropertySet)value; }
        IAudioPropertySet IFile.AudioProperties { get => AudioProperties; set => AudioProperties = (AudioPropertySet)value; }
        IDRMPropertySet IFile.DRMProperties { get => DRMProperties; set => DRMProperties = (DRMPropertySet)value; }
        IGPSPropertySet IFile.GPSProperties { get => GPSProperties; set => GPSProperties = (GPSPropertySet)value; }
        IImagePropertySet IFile.ImageProperties { get => ImageProperties; set => ImageProperties = (ImagePropertySet)value; }
        IMediaPropertySet IFile.MediaProperties { get => MediaProperties; set => MediaProperties = (MediaPropertySet)value; }
        IMusicPropertySet IFile.MusicProperties { get => MusicProperties; set => MusicProperties = (MusicPropertySet)value; }
        IPhotoPropertySet IFile.PhotoProperties { get => PhotoProperties; set => PhotoProperties = (PhotoPropertySet)value; }
        IRecordedTVPropertySet IFile.RecordedTVProperties { get => RecordedTVProperties; set => RecordedTVProperties = (RecordedTVPropertySet)value; }
        IVideoPropertySet IFile.VideoProperties { get => VideoProperties; set => VideoProperties = (VideoPropertySet)value; }
        IEnumerable<IAccessError<ILocalFile>> ILocalFile.AccessErrors => AccessErrors.Cast<IAccessError<ILocalFile>>();

        IEnumerable<IAccessError<IFile>> IFile.AccessErrors => AccessErrors.Cast<IAccessError<IFile>>();

        IEnumerable<IAccessError<ILocalDbFsItem>> ILocalDbFsItem.AccessErrors => AccessErrors.Cast<IAccessError<ILocalDbFsItem>>();

        IEnumerable<IAccessError> IDbFsItem.AccessErrors => AccessErrors.Cast<IAccessError>();

        #endregion

        public DbFile()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = AddChangeTracker(nameof(FileCrawlOptions), FileCrawlOptions.None);
            _lastAccessed = AddChangeTracker(nameof(LastAccessed), CreatedOn);
            _lastHashCalculation = AddChangeTracker<DateTime?>(nameof(LastHashCalculation), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
            _deleted = AddChangeTracker(nameof(Deleted), false);
            _creationTime = AddChangeTracker(nameof(CreationTime), CreatedOn);
            _lastWriteTime = AddChangeTracker(nameof(LastWriteTime), CreatedOn);
            _parentId = AddChangeTracker(nameof(ParentId), Guid.Empty);
            _binaryPropertySetId = AddChangeTracker(nameof(BinaryPropertySetId), Guid.Empty);
            _summaryPropertySetId = AddChangeTracker<Guid?>(nameof(SummaryPropertySetId), null);
            _documentPropertySetId = AddChangeTracker<Guid?>(nameof(DocumentPropertySetId), null);
            _audioPropertySetId = AddChangeTracker<Guid?>(nameof(AudioPropertySetId), null);
            _drmPropertySetId = AddChangeTracker<Guid?>(nameof(DRMPropertySetId), null);
            _gpsPropertySetId = AddChangeTracker<Guid?>(nameof(GPSPropertySetId), null);
            _imagePropertySetId = AddChangeTracker<Guid?>(nameof(ImagePropertySetId), null);
            _mediaPropertySetId = AddChangeTracker<Guid?>(nameof(MediaPropertySetId), null);
            _musicPropertySetId = AddChangeTracker<Guid?>(nameof(MusicPropertySetId), null);
            _photoPropertySetId = AddChangeTracker<Guid?>(nameof(PhotoPropertySetId), null);
            _recordedTVPropertySetId = AddChangeTracker<Guid?>(nameof(RecordedTVPropertySetId), null);
            _videoPropertySetId = AddChangeTracker<Guid?>(nameof(VideoPropertySetId), null);
            _parent = AddChangeTracker<Subdirectory>(nameof(Parent), null);
            _binaryPropertySet = AddChangeTracker<BinaryPropertySet>(nameof(BinaryProperties), null);
            _redundancy = AddChangeTracker<Redundancy>(nameof(Redundancy), null);
            _summaryPropertySet = AddChangeTracker<SummaryPropertySet>(nameof(SummaryProperties), null);
            _documentProperties = AddChangeTracker<DocumentPropertySet>(nameof(DocumentProperties), null);
            _audioProperties = AddChangeTracker<AudioPropertySet>(nameof(AudioProperties), null);
            _drmProperties = AddChangeTracker<DRMPropertySet>(nameof(DRMProperties), null);
            _gpsProperties = AddChangeTracker<GPSPropertySet>(nameof(GPSProperties), null);
            _imageProperties = AddChangeTracker<ImagePropertySet>(nameof(ImageProperties), null);
            _mediaProperties = AddChangeTracker<MediaPropertySet>(nameof(MediaProperties), null);
            _musicProperties = AddChangeTracker<MusicPropertySet>(nameof(MusicProperties), null);
            _photoProperties = AddChangeTracker<PhotoPropertySet>(nameof(PhotoProperties), null);
            _recordedTVProperties = AddChangeTracker<RecordedTVPropertySet>(nameof(RecordedTVProperties), null);
            _videoProperties = AddChangeTracker<VideoPropertySet>(nameof(VideoProperties), null);
        }

        internal async Task MarkDeletedAsync(LocalDbContext dbContext, CancellationToken cancellationToken, bool doNotSaveChanges = false)
        {
            if (Deleted)
                return;
            EntityEntry<DbFile> dbEntry = dbContext.Entry(this);
            FileAccessError[] fileAccessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
            if (fileAccessErrors.Length > 0)
                dbContext.FileAccessErrors.RemoveRange(fileAccessErrors);
            Deleted = true;
            if (!doNotSaveChanges)
                await dbContext.SaveChangesAsync(cancellationToken);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        internal static void BuildEntity(EntityTypeBuilder<DbFile> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.BinaryProperties).WithMany(d => d.Files).HasForeignKey(nameof(BinaryPropertySetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.SummaryProperties).WithMany(d => d.Files).HasForeignKey(nameof(SummaryPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.DocumentProperties).WithMany(d => d.Files).HasForeignKey(nameof(DocumentPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.AudioProperties).WithMany(d => d.Files).HasForeignKey(nameof(AudioPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.DRMProperties).WithMany(d => d.Files).HasForeignKey(nameof(DRMPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.GPSProperties).WithMany(d => d.Files).HasForeignKey(nameof(GPSPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.ImageProperties).WithMany(d => d.Files).HasForeignKey(nameof(ImagePropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.MediaProperties).WithMany(d => d.Files).HasForeignKey(nameof(MediaPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.MusicProperties).WithMany(d => d.Files).HasForeignKey(nameof(MusicPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.PhotoProperties).WithMany(d => d.Files).HasForeignKey(nameof(PhotoPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.RecordedTVProperties).WithMany(d => d.Files).HasForeignKey(nameof(RecordedTVPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.VideoProperties).WithMany(d => d.Files).HasForeignKey(nameof(VideoPropertySetId)).OnDelete(DeleteBehavior.Restrict);
        }

        private void ValidateLastHashCalculation(List<ValidationResult> results)
        {
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                return;
            DateTime? dateTime = LastHashCalculation;
            if (dateTime.HasValue)
            {
                if (dateTime.Value.CompareTo(CreatedOn) < 0)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastHashCalculationBeforeCreatedOn, new string[] { nameof(LastHashCalculation) }));
                else if (dateTime.Value.CompareTo(ModifiedOn) > 0)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastHashCalculationAfterModifiedOn, new string[] { nameof(LastHashCalculation) }));
            }
            else if (!(BinaryProperties is null || BinaryProperties.Hash is null))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastHashCalculationRequired, new string[] { nameof(LastHashCalculation) }));
        }

        private void ValidateName(ValidationContext validationContext, List<ValidationResult> results)
        {
            string name = Name;
            EntityEntry entry;
            LocalDbContext dbContext;
            if (string.IsNullOrEmpty(name) || (entry = validationContext.GetService<EntityEntry>()) is null ||
                (dbContext = validationContext.GetService<LocalDbContext>()) is null)
                return;
            Guid parentId = ParentId;
            if (entry.State == EntityState.Added)
            {
                if (!dbContext.Files.Any(sn => sn.ParentId == parentId && sn.Name == name))
                    return;
            }
            else
            {
                Guid id = Id;
                // TODO: Need to test whether this fails to skip an item where the id matches
                if (!dbContext.Files.Any(sn => sn.ParentId == parentId && sn.Name == name && id != sn.Id))
                    return;
            }
            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
        }

        // TODO: Change to async with LocalDbContext
        internal XElement Export(bool includeParentId = false)
        {
            XElement result = new(nameof(TABLE_NAME),
                new XAttribute(nameof(Id), XmlConvert.ToString(Id)),
                new XAttribute(nameof(Name), Name),
                new XAttribute(nameof(BinaryPropertySetId), XmlConvert.ToString(BinaryPropertySetId))
            );
            if (includeParentId)
            {
                Guid parentId = ParentId;
                if (!parentId.Equals(Guid.Empty))
                    result.SetAttributeValue(nameof(ParentId), XmlConvert.ToString(parentId));
            }
            FileCrawlOptions options = Options;
            if (options != FileCrawlOptions.None)
                result.SetAttributeValue(nameof(Options), Enum.GetName(typeof(FileCrawlOptions), Options));
            if (Deleted)
                result.SetAttributeValue(nameof(Deleted), Deleted);
            DateTime? lastHashCalculation = LastHashCalculation;
            if (lastHashCalculation.HasValue)
                result.SetAttributeValue(nameof(LastHashCalculation), XmlConvert.ToString(lastHashCalculation.Value, XmlDateTimeSerializationMode.RoundtripKind));
            result.SetAttributeValue(nameof(LastAccessed), XmlConvert.ToString(LastAccessed, XmlDateTimeSerializationMode.RoundtripKind));
            AddExportAttributes(result);
            if (Notes.Length > 0)
                result.Add(new XElement(nameof(Notes), new XCData(Notes)));
            foreach (FileAccessError accessError in AccessErrors)
                result.Add(accessError.Export());
            return result;
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateLastHashCalculation(results);
                ValidateName(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                    case nameof(LastHashCalculation):
                        ValidateLastHashCalculation(results);
                        break;
                    case nameof(Parent):
                    case nameof(Name):
                        ValidateName(validationContext, results);
                        break;
                }
        }

        internal static async Task ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid parentId, XElement fileElement)
        {
            string n = nameof(Id);
            Guid fileId = fileElement.GetAttributeGuid(n).Value;
            await new InsertQueryBuilder(nameof(LocalDbContext.Files), fileElement, n).AppendGuid(nameof(ParentId), parentId).AppendString(nameof(Name))
                .AppendGuid(nameof(BinaryPropertySetId)).AppendGuid(nameof(SummaryPropertySetId)).AppendGuid(nameof(DocumentPropertySetId)).AppendGuid(nameof(AudioPropertySetId))
                .AppendGuid(nameof(DRMPropertySetId)).AppendGuid(nameof(GPSPropertySetId)).AppendGuid(nameof(ImagePropertySetId)).AppendGuid(nameof(MediaPropertySetId))
                .AppendGuid(nameof(MusicPropertySetId)).AppendGuid(nameof(PhotoPropertySetId)).AppendGuid(nameof(RecordedTVPropertySetId))
                .AppendGuid(nameof(VideoPropertySetId)).AppendEnum<FileCrawlOptions>(nameof(Options)).AppendDateTime(nameof(LastHashCalculation))
                .AppendDateTime(nameof(LastHashCalculation)).AppendElementString(nameof(Notes)).AppendBoolean(nameof(Deleted)).AppendDateTime(nameof(CreationTime))
                .AppendDateTime(nameof(LastWriteTime)).AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            foreach (XElement accessErrorElement in fileElement.Elements(ElementName_AccessError))
                await FileAccessError.ImportAsync(dbContext, logger, fileId, accessErrorElement);
            foreach (XElement comparisonElement in fileElement.Elements(FileComparison.ELEMENT_NAME))
                await FileComparison.ImportAsync(dbContext, logger, fileId, comparisonElement);
        }

        public async Task<EntityEntry<DbFile>> RefreshAsync(LocalDbContext dbContext, long length, DateTime creationTime, DateTime lastWriteTime,
            IFileDetailProvider fileDetailProvider, bool doNotSaveChanges, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public static async Task<EntityEntry<DbFile>> AddNewAsync(LocalDbContext dbContext, Guid parentId, string name, long length, DateTime creationTime,
            DateTime lastWriteTime, IFileDetailProvider fileDetailProvider, bool doNotSaveChanges, CancellationToken cancellationToken)
        {
            DbFile file = new()
            {
                ParentId = parentId,
                Name = name,
                BinaryProperties = (await BinaryPropertySet.GetBinaryPropertySetAsync(dbContext, length, null, doNotSaveChanges, cancellationToken)).Entity,
                CreationTime = creationTime,
                LastWriteTime = lastWriteTime
            };
            EntityEntry<DbFile> result = dbContext.Files.Add(file);
            ISummaryProperties summaryPropertySet = await fileDetailProvider.GetSummaryPropertiesAsync(cancellationToken);

            if (!doNotSaveChanges)
                await dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
