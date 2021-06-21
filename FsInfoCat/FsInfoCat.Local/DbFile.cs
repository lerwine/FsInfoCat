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
        private readonly IPropertyChangeTracker<Guid> _binaryPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _summaryPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _documentPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _audioPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _drmPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _gpsPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _imagePropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _mediaPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _musicPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _photoPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _recordedTVPropertiesId;
        private readonly IPropertyChangeTracker<Guid?> _videoPropertiesId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<BinaryProperties> _content;
        private readonly IPropertyChangeTracker<Redundancy> _redundancy;
        private readonly IPropertyChangeTracker<SummaryProperties> _summaryProperties;
        private readonly IPropertyChangeTracker<DocumentProperties> _documentProperties;
        private readonly IPropertyChangeTracker<AudioProperties> _audioProperties;
        private readonly IPropertyChangeTracker<DRMProperties> _drmProperties;
        private readonly IPropertyChangeTracker<GPSProperties> _gpsProperties;
        private readonly IPropertyChangeTracker<ImageProperties> _imageProperties;
        private readonly IPropertyChangeTracker<MediaProperties> _mediaProperties;
        private readonly IPropertyChangeTracker<MusicProperties> _musicProperties;
        private readonly IPropertyChangeTracker<PhotoProperties> _photoProperties;
        private readonly IPropertyChangeTracker<RecordedTVProperties> _recordedTVProperties;
        private readonly IPropertyChangeTracker<VideoProperties> _videoProperties;
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

        public virtual Guid BinaryPropertiesId
        {
            get => _binaryPropertiesId.GetValue();
            set
            {
                if (_binaryPropertiesId.SetValue(value))
                {
                    BinaryProperties nav = _content.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? SummaryPropertiesId
        {
            get => _summaryPropertiesId.GetValue();
            set
            {
                if (_summaryPropertiesId.SetValue(value))
                {
                    SummaryProperties nav = _summaryProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? DocumentPropertiesId
        {
            get => _documentPropertiesId.GetValue();
            set
            {
                if (_documentPropertiesId.SetValue(value))
                {
                    DocumentProperties nav = _documentProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? AudioPropertiesId
        {
            get => _audioPropertiesId.GetValue();
            set
            {
                if (_audioPropertiesId.SetValue(value))
                {
                    AudioProperties nav = _audioProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? DRMPropertiesId
        {
            get => _drmPropertiesId.GetValue();
            set
            {
                if (_drmPropertiesId.SetValue(value))
                {
                    DRMProperties nav = _drmProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? GPSPropertiesId
        {
            get => _gpsPropertiesId.GetValue();
            set
            {
                if (_gpsPropertiesId.SetValue(value))
                {
                    GPSProperties nav = _gpsProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? ImagePropertiesId
        {
            get => _imagePropertiesId.GetValue();
            set
            {
                if (_imagePropertiesId.SetValue(value))
                {
                    ImageProperties nav = _imageProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? MediaPropertiesId
        {
            get => _mediaPropertiesId.GetValue();
            set
            {
                if (_mediaPropertiesId.SetValue(value))
                {
                    MediaProperties nav = _mediaProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? MusicPropertiesId
        {
            get => _musicPropertiesId.GetValue();
            set
            {
                if (_musicPropertiesId.SetValue(value))
                {
                    MusicProperties nav = _musicProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? PhotoPropertiesId
        {
            get => _photoPropertiesId.GetValue();
            set
            {
                if (_photoPropertiesId.SetValue(value))
                {
                    PhotoProperties nav = _photoProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? RecordedTVPropertiesId
        {
            get => _recordedTVPropertiesId.GetValue();
            set
            {
                if (_recordedTVPropertiesId.SetValue(value))
                {
                    RecordedTVProperties nav = _recordedTVProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? VideoPropertiesId
        {
            get => _videoPropertiesId.GetValue();
            set
            {
                if (_videoPropertiesId.SetValue(value))
                {
                    VideoProperties nav = _videoProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }


        public virtual BinaryProperties BinaryProperties
        {
            get => _content.GetValue();
            set
            {
                if (_content.SetValue(value))
                {
                    if (value is null)
                        _binaryPropertiesId.SetValue(Guid.Empty);
                    else
                        _binaryPropertiesId.SetValue(value.Id);
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
        public virtual SummaryProperties SummaryProperties
        {
            get => _summaryProperties.GetValue();
            set
            {
                if (_summaryProperties.SetValue(value))
                {
                    if (value is null)
                        _summaryPropertiesId.SetValue(null);
                    else
                        _summaryPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DocumentProperties DocumentProperties
        {
            get => _documentProperties.GetValue();
            set
            {
                if (_documentProperties.SetValue(value))
                {
                    if (value is null)
                        _documentPropertiesId.SetValue(null);
                    else
                        _documentPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual AudioProperties AudioProperties
        {
            get => _audioProperties.GetValue();
            set
            {
                if (_audioProperties.SetValue(value))
                {
                    if (value is null)
                        _audioPropertiesId.SetValue(null);
                    else
                        _audioPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DRMProperties DRMProperties
        {
            get => _drmProperties.GetValue();
            set
            {
                if (_drmProperties.SetValue(value))
                {
                    if (value is null)
                        _drmPropertiesId.SetValue(null);
                    else
                        _drmPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual GPSProperties GPSProperties
        {
            get => _gpsProperties.GetValue();
            set
            {
                if (_gpsProperties.SetValue(value))
                {
                    if (value is null)
                        _gpsPropertiesId.SetValue(null);
                    else
                        _gpsPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ImageProperties ImageProperties
        {
            get => _imageProperties.GetValue();
            set
            {
                if (_imageProperties.SetValue(value))
                {
                    if (value is null)
                        _imagePropertiesId.SetValue(null);
                    else
                        _imagePropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MediaProperties MediaProperties
        {
            get => _mediaProperties.GetValue();
            set
            {
                if (_mediaProperties.SetValue(value))
                {
                    if (value is null)
                        _mediaPropertiesId.SetValue(null);
                    else
                        _mediaPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MusicProperties MusicProperties
        {
            get => _musicProperties.GetValue();
            set
            {
                if (_musicProperties.SetValue(value))
                {
                    if (value is null)
                        _musicPropertiesId.SetValue(null);
                    else
                        _musicPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual PhotoProperties PhotoProperties
        {
            get => _photoProperties.GetValue();
            set
            {
                if (_photoProperties.SetValue(value))
                {
                    if (value is null)
                        _photoPropertiesId.SetValue(null);
                    else
                        _photoPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RecordedTVProperties RecordedTVProperties
        {
            get => _recordedTVProperties.GetValue();
            set
            {
                if (_recordedTVProperties.SetValue(value))
                {
                    if (value is null)
                        _recordedTVPropertiesId.SetValue(null);
                    else
                        _recordedTVPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VideoProperties VideoProperties
        {
            get => _videoProperties.GetValue();
            set
            {
                if (_videoProperties.SetValue(value))
                {
                    if (value is null)
                        _videoPropertiesId.SetValue(null);
                    else
                        _videoPropertiesId.SetValue(value.Id);
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

        ILocalBinaryProperties ILocalFile.BinaryProperties { get => BinaryProperties; set => BinaryProperties = (BinaryProperties)value; }

        IBinaryProperties IFile.BinaryProperties { get => BinaryProperties; set => BinaryProperties = (BinaryProperties)value; }

        ILocalSubdirectory ILocalFile.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ISubdirectory IDbFsItem.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        IEnumerable<ILocalComparison> ILocalFile.ComparisonSources => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonSources => ComparisonSources.Cast<IComparison>();

        IEnumerable<ILocalComparison> ILocalFile.ComparisonTargets => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonTargets => ComparisonSources.Cast<IComparison>();

        ILocalSummaryProperties ILocalFile.SummaryProperties { get => SummaryProperties; set => SummaryProperties = (SummaryProperties)value; }
        ILocalDocumentProperties ILocalFile.DocumentProperties { get => DocumentProperties; set => DocumentProperties = (DocumentProperties)value; }
        ILocalAudioProperties ILocalFile.AudioProperties { get => AudioProperties; set => AudioProperties = (AudioProperties)value; }
        ILocalDRMProperties ILocalFile.DRMProperties { get => DRMProperties; set => DRMProperties = (DRMProperties)value; }
        ILocalGPSProperties ILocalFile.GPSProperties { get => GPSProperties; set => GPSProperties = (GPSProperties)value; }
        ILocalImageProperties ILocalFile.ImageProperties { get => ImageProperties; set => ImageProperties = (ImageProperties)value; }
        ILocalMediaProperties ILocalFile.MediaProperties { get => MediaProperties; set => MediaProperties = (MediaProperties)value; }
        ILocalMusicProperties ILocalFile.MusicProperties { get => MusicProperties; set => MusicProperties = (MusicProperties)value; }
        ILocalPhotoProperties ILocalFile.PhotoProperties { get => PhotoProperties; set => PhotoProperties = (PhotoProperties)value; }
        ILocalRecordedTVProperties ILocalFile.RecordedTVProperties { get => RecordedTVProperties; set => RecordedTVProperties = (RecordedTVProperties)value; }
        ILocalVideoProperties ILocalFile.VideoProperties { get => VideoProperties; set => VideoProperties = (VideoProperties)value; }
        ISummaryProperties IFile.SummaryProperties { get => SummaryProperties; set => SummaryProperties = (SummaryProperties)value; }
        IDocumentProperties IFile.DocumentProperties { get => DocumentProperties; set => DocumentProperties = (DocumentProperties)value; }
        IAudioProperties IFile.AudioProperties { get => AudioProperties; set => AudioProperties = (AudioProperties)value; }
        IDRMProperties IFile.DRMProperties { get => DRMProperties; set => DRMProperties = (DRMProperties)value; }
        IGPSProperties IFile.GPSProperties { get => GPSProperties; set => GPSProperties = (GPSProperties)value; }
        IImageProperties IFile.ImageProperties { get => ImageProperties; set => ImageProperties = (ImageProperties)value; }
        IMediaProperties IFile.MediaProperties { get => MediaProperties; set => MediaProperties = (MediaProperties)value; }
        IMusicProperties IFile.MusicProperties { get => MusicProperties; set => MusicProperties = (MusicProperties)value; }
        IPhotoProperties IFile.PhotoProperties { get => PhotoProperties; set => PhotoProperties = (PhotoProperties)value; }
        IRecordedTVProperties IFile.RecordedTVProperties { get => RecordedTVProperties; set => RecordedTVProperties = (RecordedTVProperties)value; }
        IVideoProperties IFile.VideoProperties { get => VideoProperties; set => VideoProperties = (VideoProperties)value; }
        IExtendedProperties IFile.ExtendedProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ILocalExtendedProperties ILocalFile.ExtendedProperties { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            _binaryPropertiesId = AddChangeTracker(nameof(BinaryPropertiesId), Guid.Empty);
            _summaryPropertiesId = AddChangeTracker<Guid?>(nameof(SummaryPropertiesId), null);
            _documentPropertiesId = AddChangeTracker<Guid?>(nameof(DocumentPropertiesId), null);
            _audioPropertiesId = AddChangeTracker<Guid?>(nameof(AudioPropertiesId), null);
            _drmPropertiesId = AddChangeTracker<Guid?>(nameof(DRMPropertiesId), null);
            _gpsPropertiesId = AddChangeTracker<Guid?>(nameof(GPSPropertiesId), null);
            _imagePropertiesId = AddChangeTracker<Guid?>(nameof(ImagePropertiesId), null);
            _mediaPropertiesId = AddChangeTracker<Guid?>(nameof(MediaPropertiesId), null);
            _musicPropertiesId = AddChangeTracker<Guid?>(nameof(MusicPropertiesId), null);
            _photoPropertiesId = AddChangeTracker<Guid?>(nameof(PhotoPropertiesId), null);
            _recordedTVPropertiesId = AddChangeTracker<Guid?>(nameof(RecordedTVPropertiesId), null);
            _videoPropertiesId = AddChangeTracker<Guid?>(nameof(VideoPropertiesId), null);
            _parent = AddChangeTracker<Subdirectory>(nameof(Parent), null);
            _content = AddChangeTracker<BinaryProperties>(nameof(BinaryProperties), null);
            _redundancy = AddChangeTracker<Redundancy>(nameof(Redundancy), null);
            _summaryProperties = AddChangeTracker<SummaryProperties>(nameof(SummaryProperties), null);
            _documentProperties = AddChangeTracker<DocumentProperties>(nameof(DocumentProperties), null);
            _audioProperties = AddChangeTracker<AudioProperties>(nameof(AudioProperties), null);
            _drmProperties = AddChangeTracker<DRMProperties>(nameof(DRMProperties), null);
            _gpsProperties = AddChangeTracker<GPSProperties>(nameof(GPSProperties), null);
            _imageProperties = AddChangeTracker<ImageProperties>(nameof(ImageProperties), null);
            _mediaProperties = AddChangeTracker<MediaProperties>(nameof(MediaProperties), null);
            _musicProperties = AddChangeTracker<MusicProperties>(nameof(MusicProperties), null);
            _photoProperties = AddChangeTracker<PhotoProperties>(nameof(PhotoProperties), null);
            _recordedTVProperties = AddChangeTracker<RecordedTVProperties>(nameof(RecordedTVProperties), null);
            _videoProperties = AddChangeTracker<VideoProperties>(nameof(VideoProperties), null);
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
            builder.HasOne(sn => sn.BinaryProperties).WithMany(d => d.Files).HasForeignKey(nameof(BinaryPropertiesId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.SummaryProperties).WithMany(d => d.Files).HasForeignKey(nameof(SummaryPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.DocumentProperties).WithMany(d => d.Files).HasForeignKey(nameof(DocumentPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.AudioProperties).WithMany(d => d.Files).HasForeignKey(nameof(AudioPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.DRMProperties).WithMany(d => d.Files).HasForeignKey(nameof(DRMPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.GPSProperties).WithMany(d => d.Files).HasForeignKey(nameof(GPSPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.ImageProperties).WithMany(d => d.Files).HasForeignKey(nameof(ImagePropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.MediaProperties).WithMany(d => d.Files).HasForeignKey(nameof(MediaPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.MusicProperties).WithMany(d => d.Files).HasForeignKey(nameof(MusicPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.PhotoProperties).WithMany(d => d.Files).HasForeignKey(nameof(PhotoPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.RecordedTVProperties).WithMany(d => d.Files).HasForeignKey(nameof(RecordedTVPropertiesId)).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.VideoProperties).WithMany(d => d.Files).HasForeignKey(nameof(VideoPropertiesId)).OnDelete(DeleteBehavior.Restrict);
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
                new XAttribute(nameof(BinaryPropertiesId), XmlConvert.ToString(BinaryPropertiesId))
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
                .AppendGuid(nameof(BinaryPropertiesId)).AppendGuid(nameof(SummaryPropertiesId)).AppendGuid(nameof(DocumentPropertiesId)).AppendGuid(nameof(AudioPropertiesId))
                .AppendGuid(nameof(DRMPropertiesId)).AppendGuid(nameof(GPSPropertiesId)).AppendGuid(nameof(ImagePropertiesId)).AppendGuid(nameof(MediaPropertiesId))
                .AppendGuid(nameof(MusicPropertiesId)).AppendGuid(nameof(PhotoPropertiesId)).AppendGuid(nameof(RecordedTVPropertiesId))
                .AppendGuid(nameof(VideoPropertiesId)).AppendEnum<FileCrawlOptions>(nameof(Options)).AppendDateTime(nameof(LastHashCalculation))
                .AppendDateTime(nameof(LastHashCalculation)).AppendElementString(nameof(Notes)).AppendBoolean(nameof(Deleted)).AppendDateTime(nameof(CreationTime))
                .AppendDateTime(nameof(LastWriteTime)).AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            foreach (XElement accessErrorElement in fileElement.Elements(ElementName_AccessError))
                await FileAccessError.ImportAsync(dbContext, logger, fileId, accessErrorElement);
            foreach (XElement comparisonElement in fileElement.Elements(FileComparison.ELEMENT_NAME))
                await FileComparison.ImportAsync(dbContext, logger, fileId, comparisonElement);
        }
    }
}
