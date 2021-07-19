using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class DbFile : LocalDbEntity, ILocalFile
    {
        #region Fields

        public const string TABLE_NAME = "Files";

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<FileCorrelationStatus> _status;
        private readonly IPropertyChangeTracker<FileCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<DateTime?> _lastHashCalculation;
        private readonly IPropertyChangeTracker<string> _notes;
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
        private readonly IPropertyChangeTracker<BinaryPropertySet> _binaryProperties;
        private readonly IPropertyChangeTracker<Redundancy> _redundancy;
        private readonly IPropertyChangeTracker<SummaryPropertySet> _summaryProperties;
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
        private HashSet<FileComparison> _baselineComparisons = new();
        private HashSet<FileComparison> _correlativeComparisons = new();

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
        public virtual FileCorrelationStatus Status { get => _status.GetValue(); set => _status.SetValue(value); }

        [Required]
        public virtual FileCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        public virtual DateTime? LastHashCalculation { get => _lastHashCalculation.GetValue(); set => _lastHashCalculation.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

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
                    BinaryPropertySet nav = _binaryProperties.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _binaryProperties.SetValue(null);
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
                    SummaryPropertySet nav = _summaryProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
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
                            _binaryProperties.SetValue(null);
                    }
                    else if (!(nav is null))
                        _binaryProperties.SetValue(null);
                }
            }
        }


        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties.GetValue();
            set
            {
                if (_binaryProperties.SetValue(value))
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
            get => _summaryProperties.GetValue();
            set
            {
                if (_summaryProperties.SetValue(value))
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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> BaselineComparisons
        {
            get => _baselineComparisons;
            set => CheckHashSetChanged(_baselineComparisons, value, h => _baselineComparisons = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> CorrelativeComparisons
        {
            get => _correlativeComparisons;
            set => CheckHashSetChanged(_correlativeComparisons, value, h => _correlativeComparisons = h);
        }

        #endregion

        #region Explicit Members

        ILocalBinaryPropertySet ILocalFile.BinaryProperties { get => BinaryProperties; }

        IBinaryPropertySet IFile.BinaryProperties { get => BinaryProperties; }

        ISubdirectory IDbFsItem.Parent { get => Parent; }

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        IEnumerable<ILocalComparison> ILocalFile.BaselineComparisons => BaselineComparisons.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.BaselineComparisons => BaselineComparisons.Cast<IComparison>();

        IEnumerable<ILocalComparison> ILocalFile.CorrelativeComparisons => CorrelativeComparisons.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.CorrelativeComparisons => CorrelativeComparisons.Cast<IComparison>();

        ILocalSummaryPropertySet ILocalFile.SummaryProperties { get => SummaryProperties; }
        ILocalDocumentPropertySet ILocalFile.DocumentProperties { get => DocumentProperties; }
        ILocalAudioPropertySet ILocalFile.AudioProperties { get => AudioProperties; }
        ILocalDRMPropertySet ILocalFile.DRMProperties { get => DRMProperties; }
        ILocalGPSPropertySet ILocalFile.GPSProperties { get => GPSProperties; }
        ILocalImagePropertySet ILocalFile.ImageProperties { get => ImageProperties; }
        ILocalMediaPropertySet ILocalFile.MediaProperties { get => MediaProperties; }
        ILocalMusicPropertySet ILocalFile.MusicProperties { get => MusicProperties; }
        ILocalPhotoPropertySet ILocalFile.PhotoProperties { get => PhotoProperties; }
        ILocalRecordedTVPropertySet ILocalFile.RecordedTVProperties { get => RecordedTVProperties; }
        ILocalVideoPropertySet ILocalFile.VideoProperties { get => VideoProperties; }
        ISummaryPropertySet IFile.SummaryProperties { get => SummaryProperties; }
        IDocumentPropertySet IFile.DocumentProperties { get => DocumentProperties; }
        IAudioPropertySet IFile.AudioProperties { get => AudioProperties; }
        IDRMPropertySet IFile.DRMProperties { get => DRMProperties; }
        IGPSPropertySet IFile.GPSProperties { get => GPSProperties; }
        IImagePropertySet IFile.ImageProperties { get => ImageProperties; }
        IMediaPropertySet IFile.MediaProperties { get => MediaProperties; }
        IMusicPropertySet IFile.MusicProperties { get => MusicProperties; }
        IPhotoPropertySet IFile.PhotoProperties { get => PhotoProperties; }
        IRecordedTVPropertySet IFile.RecordedTVProperties { get => RecordedTVProperties; }
        IVideoPropertySet IFile.VideoProperties { get => VideoProperties; }

        IEnumerable<ILocalFileAccessError> ILocalFile.AccessErrors => AccessErrors.Cast<ILocalFileAccessError>();

        ILocalSubdirectory ILocalDbFsItem.Parent => Parent;

        IEnumerable<ILocalAccessError> ILocalDbFsItem.AccessErrors => AccessErrors.Cast<ILocalFileAccessError>();

        IEnumerable<IFileAccessError> IFile.AccessErrors => AccessErrors.Cast<ILocalFileAccessError>();

        IEnumerable<IAccessError> IDbFsItem.AccessErrors => AccessErrors.Cast<IAccessError>();

        #endregion

        public DbFile()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _status = AddChangeTracker(nameof(FileCorrelationStatus), FileCorrelationStatus.Dissociated);
            _options = AddChangeTracker(nameof(FileCrawlOptions), FileCrawlOptions.None);
            _lastAccessed = AddChangeTracker(nameof(LastAccessed), CreatedOn);
            _lastHashCalculation = AddChangeTracker<DateTime?>(nameof(LastHashCalculation), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonWhiteSpaceOrEmptyStringCoersion.Default);
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
            _binaryProperties = AddChangeTracker<BinaryPropertySet>(nameof(BinaryProperties), null);
            _redundancy = AddChangeTracker<Redundancy>(nameof(Redundancy), null);
            _summaryProperties = AddChangeTracker<SummaryPropertySet>(nameof(SummaryProperties), null);
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

        /// <summary>Asynchronously expunghes the current <see cref="Subdirectory"/> from the database, including all subdirectories and files contained therein,
        /// if the <see cref="Status"/> is <see cref="DirectoryStatus.Deleted"/> and <see cref="UpstreamId"/> is <see langword="null"/>.</summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{TResult}">Task&lt;System.Boolean&gt;</see> returning <span class="keyword"><span class="languageSpecificText"><span class="cs">true</span><span class="vb">True</span><span class="cpp">true</span></span></span><span class="nu"><span class="keyword">true</span> (<span class="keyword">True</span> in Visual Basic)</span>
        /// if the subdirectory was deleted; otherwise <span class="keyword"><span class="languageSpecificText"><span class="cs">false</span><span class="vb">False</span><span class="cpp">false</span></span></span><span class="nu"><span class="keyword">false</span> (<span class="keyword">False</span> in Visual Basic)</span>
        /// if <see cref="Status"/> was not <see cref="FileCorrelationStatus.Deleted"/> or <see cref="UpstreamId"/> was not null.</returns>
        public async Task<bool> ExpungeAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (dbContext.Database.CurrentTransaction is null)
            {
                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                bool result = await ExpungeAsync(dbContext, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if (result)
                    await transaction.CommitAsync(cancellationToken);
                else
                    await transaction.RollbackAsync(cancellationToken);
                return result;
            }
            EntityEntry<DbFile> dbEntry = dbContext.Entry(this);
            if (Status != FileCorrelationStatus.Deleted || UpstreamId.HasValue || !dbEntry.ExistsInDb())
                return false;
            return await ExpungeAsync(dbEntry, cancellationToken);
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

        public async Task<EntityEntry<DbFile>> RefreshAsync(LocalDbContext dbContext, long length, DateTime creationTime, DateTime lastWriteTime,
            IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (dbContext.Database.CurrentTransaction is null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                EntityEntry<DbFile> result = await RefreshAsync(dbContext, length, creationTime, lastWriteTime, fileDetailProvider, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }

            EntityEntry<DbFile> entry = dbContext.Entry(this);
            switch (entry.State)
            {
                case EntityState.Detached:
                    throw new InvalidOperationException("File entity is detached from the database context.");
                case EntityState.Added:
                    throw new InvalidOperationException("New file entity has not been saved to the database.");
                case EntityState.Deleted:
                    throw new InvalidOperationException("File entity is flagged to be deleted from the database.");
                case EntityState.Modified:
                    throw new InvalidOperationException("Previous file entity changes have not been saved to the database.");
            }
            BinaryPropertySet oldBinaryProperties = BinaryProperties ?? await entry.GetRelatedReferenceAsync(f => f.BinaryProperties, cancellationToken);
            DateTime oldCreationTime = CreationTime;
            DateTime oldLastWriteTime = LastWriteTime;
            CreationTime = creationTime;
            LastWriteTime = lastWriteTime;

            if (length != oldBinaryProperties.Length || ((length == 0L) ? !oldBinaryProperties.Hash.HasValue : (entry.State == EntityState.Modified && oldBinaryProperties.Hash.HasValue)))
            {
                BinaryProperties = await BinaryPropertySet.GetBinaryPropertySetAsync(dbContext, 0L, cancellationToken);
                Status = FileCorrelationStatus.Dissociated;
            }

            if (Options.HasFlag(FileCrawlOptions.Ignore))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (entry.State == EntityState.Unchanged)
                {
                    if (Status == FileCorrelationStatus.Correlated)
                        return entry;
                    Status = FileCorrelationStatus.Correlated;
                }
                entry = dbContext.Files.Update(this);
                await dbContext.SaveChangesAsync(cancellationToken);
                return entry;
            }

            await SummaryPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await AudioPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await DocumentPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await DRMPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await GPSPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await ImagePropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await MediaPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await MusicPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await PhotoPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await RecordedTVPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await VideoPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);

            if (entry.State != EntityState.Unchanged)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Status = FileCorrelationStatus.Dissociated;
                entry = dbContext.Files.Update(this);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return entry;
        }

        public static async Task<EntityEntry<DbFile>> AddNewAsync(LocalDbContext dbContext, Guid parentId, string name, long length, DateTime creationTime,
            DateTime lastWriteTime, IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (dbContext.Database.CurrentTransaction is null)
            {
                using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                EntityEntry<DbFile> result = await AddNewAsync(dbContext, parentId, name, length, creationTime, lastWriteTime, fileDetailProvider, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            DbFile file = new()
            {
                ParentId = parentId,
                Name = name,
                BinaryProperties = await BinaryPropertySet.GetBinaryPropertySetAsync(dbContext, length, cancellationToken),
                CreationTime = creationTime,
                LastWriteTime = lastWriteTime
            };
            EntityEntry<DbFile> entry = dbContext.Files.Add(file);
            await SummaryPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await AudioPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await DocumentPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await DRMPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await GPSPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await ImagePropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await MediaPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await MusicPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await PhotoPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await RecordedTVPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await VideoPropertySet.RefreshAsync(entry, fileDetailProvider, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            // TODO: Need to add code later to see where comparisons need to be made
            return entry;
        }

        //private static async Task MarkDissociatedAsync(EntityEntry<DbFile> dbEntry, CancellationToken cancellationToken)
        //{
        //    if (dbEntry.Context is not LocalDbContext dbContext)
        //        throw new InvalidOperationException();
        //    Redundancy oldRedundancy = await dbEntry.GetRelatedReferenceAsync(f => f.Redundancy, cancellationToken);
        //    EntityEntry<RedundantSet> oldRedundantSet;
        //    if (oldRedundancy is null)
        //        oldRedundantSet = null;
        //    else
        //    {
        //        oldRedundantSet = await dbContext.Entry(oldRedundancy).GetRelatedTargetEntryAsync(r => r.RedundantSet, cancellationToken);
        //        cancellationToken.ThrowIfCancellationRequested();
        //        dbContext.Redundancies.Remove(oldRedundancy);
        //        await dbContext.SaveChangesAsync(cancellationToken);
        //        dbEntry.Entity.Redundancy = null;
        //    }
        //    EntityEntry<BinaryPropertySet> oldBinaryPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.BinaryProperties, cancellationToken);
        //    if (oldBinaryPropertySet.Entity.Hash.HasValue || oldBinaryPropertySet.Entity.Length > 0)
        //    {
        //        BinaryPropertySet newBinaryPropertySet = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == 0L && p.Hash == null, cancellationToken);
        //        if (newBinaryPropertySet is null)
        //        {
        //            newBinaryPropertySet = new() { Length = 0L };
        //            cancellationToken.ThrowIfCancellationRequested();
        //            dbContext.BinaryPropertySets.Add(newBinaryPropertySet);
        //            await dbContext.SaveChangesAsync(cancellationToken);
        //        }
        //        dbEntry.Entity.BinaryProperties = newBinaryPropertySet;
        //    }
        //    FileAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
        //    cancellationToken.ThrowIfCancellationRequested();
        //    if (accessErrors.Length > 0)
        //        dbContext.FileAccessErrors.RemoveRange(accessErrors);
        //    FileComparison[] comparisons = (await dbEntry.GetRelatedCollectionAsync(f => f.BaselineComparisons, cancellationToken))
        //        .Concat(await dbEntry.GetRelatedCollectionAsync(f => f.CorrelativeComparisons, cancellationToken)).ToArray();
        //    cancellationToken.ThrowIfCancellationRequested();
        //    if (comparisons.Length > 0)
        //        dbContext.Comparisons.RemoveRange(comparisons);
        //    EntityEntry<SummaryPropertySet> oldSummaryPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.SummaryProperties, cancellationToken);
        //    dbEntry.Entity.SummaryProperties = null;
        //    EntityEntry<AudioPropertySet> oldAudioPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.AudioProperties, cancellationToken);
        //    dbEntry.Entity.AudioProperties = null;
        //    EntityEntry<DocumentPropertySet> oldDocumentPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.DocumentProperties, cancellationToken);
        //    dbEntry.Entity.DocumentProperties = null;
        //    EntityEntry<DRMPropertySet> oldDRMPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.DRMProperties, cancellationToken);
        //    dbEntry.Entity.DRMProperties = null;
        //    EntityEntry<GPSPropertySet> oldGPSPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.GPSProperties, cancellationToken);
        //    dbEntry.Entity.GPSProperties = null;
        //    EntityEntry<ImagePropertySet> oldImagePropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.ImageProperties, cancellationToken);
        //    dbEntry.Entity.ImageProperties = null;
        //    EntityEntry<MediaPropertySet> oldMediaPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.MediaProperties, cancellationToken);
        //    dbEntry.Entity.MediaProperties = null;
        //    EntityEntry<MusicPropertySet> oldMusicPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.MusicProperties, cancellationToken);
        //    dbEntry.Entity.MusicProperties = null;
        //    EntityEntry<PhotoPropertySet> oldPhotoPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.PhotoProperties, cancellationToken);
        //    dbEntry.Entity.PhotoProperties = null;
        //    EntityEntry<RecordedTVPropertySet> oldRecordedTVPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.RecordedTVProperties, cancellationToken);
        //    dbEntry.Entity.RecordedTVProperties = null;
        //    EntityEntry<VideoPropertySet> oldVideoPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.VideoProperties, cancellationToken);
        //    dbEntry.Entity.SummaryProperties = null;
        //    dbEntry.Entity.Status = FileCorrelationStatus.Deleted;
        //    switch (dbEntry.Entity.Options)
        //    {
        //        case FileCrawlOptions.FlaggedForDeletion:
        //        case FileCrawlOptions.FlaggedForRescan:
        //            dbEntry.Entity.Options = FileCrawlOptions.None;
        //            break;
        //    }
        //    Guid id = dbEntry.Entity.Id;
        //    cancellationToken.ThrowIfCancellationRequested();
        //    await dbContext.SaveChangesAsync(cancellationToken);
        //    bool shouldSaveChanges = oldRedundantSet.ExistsInDb() && !(await oldRedundantSet.GetRelatedCollectionAsync(r => r.Redundancies, cancellationToken)).Any(r => r.FileId != id);
        //    if (shouldSaveChanges)
        //        dbContext.RedundantSets.Remove(oldRedundantSet.Entity);
        //    if (oldBinaryPropertySet.ExistsInDb() && oldBinaryPropertySet.Entity.Hash.HasValue || oldBinaryPropertySet.Entity.Length > 0 &&
        //        !(await oldBinaryPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.BinaryPropertySets.Remove(oldBinaryPropertySet.Entity);
        //    }
        //    if (oldSummaryPropertySet.ExistsInDb() && (await oldSummaryPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.SummaryPropertySets.Remove(oldSummaryPropertySet.Entity);
        //    }
        //    if (oldAudioPropertySet.ExistsInDb() && (await oldAudioPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.AudioPropertySets.Remove(oldAudioPropertySet.Entity);
        //    }
        //    if (oldDocumentPropertySet.ExistsInDb() && (await oldDocumentPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.DocumentPropertySets.Remove(oldDocumentPropertySet.Entity);
        //    }
        //    if (oldDRMPropertySet.ExistsInDb() && (await oldDRMPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.DRMPropertySets.Remove(oldDRMPropertySet.Entity);
        //    }
        //    if (oldGPSPropertySet.ExistsInDb() && (await oldGPSPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.GPSPropertySets.Remove(oldGPSPropertySet.Entity);
        //    }
        //    if (oldImagePropertySet.ExistsInDb() && (await oldImagePropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.ImagePropertySets.Remove(oldImagePropertySet.Entity);
        //    }
        //    if (oldMediaPropertySet.ExistsInDb() && (await oldMediaPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.MediaPropertySets.Remove(oldMediaPropertySet.Entity);
        //    }
        //    if (oldMusicPropertySet.ExistsInDb() && (await oldMusicPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.MusicPropertySets.Remove(oldMusicPropertySet.Entity);
        //    }
        //    if (oldPhotoPropertySet.ExistsInDb() && (await oldPhotoPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.PhotoPropertySets.Remove(oldPhotoPropertySet.Entity);
        //    }
        //    if (oldRecordedTVPropertySet.ExistsInDb() && (await oldRecordedTVPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.RecordedTVPropertySets.Remove(oldRecordedTVPropertySet.Entity);
        //    }
        //    if (oldVideoPropertySet.ExistsInDb() && (await oldVideoPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
        //    {
        //        shouldSaveChanges = true;
        //        dbContext.VideoPropertySets.Remove(oldVideoPropertySet.Entity);
        //    }
        //    cancellationToken.ThrowIfCancellationRequested();
        //    if (shouldSaveChanges)
        //        await dbContext.SaveChangesAsync(cancellationToken);
        //}

        //public async Task MarkDissociatedAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();
        //    EntityEntry<DbFile> dbEntry = dbContext.Entry(this);
        //    switch (dbEntry.State)
        //    {
        //        case EntityState.Detached:
        //        case EntityState.Deleted:
        //            throw new InvalidOperationException();
        //    }
        //    if (Status == FileCorrelationStatus.Dissociated)
        //        return;
        //    if (dbContext.Database.CurrentTransaction is null)
        //    {
        //        IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        //        cancellationToken.ThrowIfCancellationRequested();
        //        await MarkDissociatedAsync(dbEntry, cancellationToken);
        //        cancellationToken.ThrowIfCancellationRequested();
        //        await transaction.CommitAsync(cancellationToken);
        //    }
        //    else
        //        await MarkDissociatedAsync(dbEntry, cancellationToken);
        //}

        private static async Task SetStatusDeleted(EntityEntry<DbFile> dbEntry, CancellationToken cancellationToken)
        {
            if (dbEntry.Context is not LocalDbContext dbContext)
                throw new InvalidOperationException();
            Redundancy oldRedundancy = await dbEntry.GetRelatedReferenceAsync(f => f.Redundancy, cancellationToken);
            EntityEntry<RedundantSet> oldRedundantSet;
            if (oldRedundancy is null)
                oldRedundantSet = null;
            else
            {
                oldRedundantSet = await dbContext.Entry(oldRedundancy).GetRelatedTargetEntryAsync(r => r.RedundantSet, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                dbContext.Redundancies.Remove(oldRedundancy);
                await dbContext.SaveChangesAsync(cancellationToken);
                dbEntry.Entity.Redundancy = null;
            }
            EntityEntry<BinaryPropertySet> oldBinaryPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.BinaryProperties, cancellationToken);
            if (oldBinaryPropertySet.Entity.Hash.HasValue || oldBinaryPropertySet.Entity.Length > 0)
            {
                BinaryPropertySet newBinaryPropertySet = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == 0L && p.Hash == null, cancellationToken);
                if (newBinaryPropertySet is null)
                {
                    newBinaryPropertySet = new() { Length = 0L };
                    cancellationToken.ThrowIfCancellationRequested();
                    dbContext.BinaryPropertySets.Add(newBinaryPropertySet);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                dbEntry.Entity.BinaryProperties = newBinaryPropertySet;
            }
            FileAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (accessErrors.Length > 0)
                dbContext.FileAccessErrors.RemoveRange(accessErrors);
            FileComparison[] comparisons = (await dbEntry.GetRelatedCollectionAsync(f => f.BaselineComparisons, cancellationToken))
                .Concat(await dbEntry.GetRelatedCollectionAsync(f => f.CorrelativeComparisons, cancellationToken)).ToArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (comparisons.Length > 0)
                dbContext.Comparisons.RemoveRange(comparisons);
            EntityEntry<SummaryPropertySet> oldSummaryPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.SummaryProperties, cancellationToken);
            dbEntry.Entity.SummaryProperties = null;
            EntityEntry<AudioPropertySet> oldAudioPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.AudioProperties, cancellationToken);
            dbEntry.Entity.AudioProperties = null;
            EntityEntry<DocumentPropertySet> oldDocumentPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.DocumentProperties, cancellationToken);
            dbEntry.Entity.DocumentProperties = null;
            EntityEntry<DRMPropertySet> oldDRMPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.DRMProperties, cancellationToken);
            dbEntry.Entity.DRMProperties = null;
            EntityEntry<GPSPropertySet> oldGPSPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.GPSProperties, cancellationToken);
            dbEntry.Entity.GPSProperties = null;
            EntityEntry<ImagePropertySet> oldImagePropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.ImageProperties, cancellationToken);
            dbEntry.Entity.ImageProperties = null;
            EntityEntry<MediaPropertySet> oldMediaPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.MediaProperties, cancellationToken);
            dbEntry.Entity.MediaProperties = null;
            EntityEntry<MusicPropertySet> oldMusicPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.MusicProperties, cancellationToken);
            dbEntry.Entity.MusicProperties = null;
            EntityEntry<PhotoPropertySet> oldPhotoPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.PhotoProperties, cancellationToken);
            dbEntry.Entity.PhotoProperties = null;
            EntityEntry<RecordedTVPropertySet> oldRecordedTVPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.RecordedTVProperties, cancellationToken);
            dbEntry.Entity.RecordedTVProperties = null;
            EntityEntry<VideoPropertySet> oldVideoPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.VideoProperties, cancellationToken);
            dbEntry.Entity.SummaryProperties = null;
            dbEntry.Entity.Status = FileCorrelationStatus.Deleted;
            switch (dbEntry.Entity.Options)
            {
                case FileCrawlOptions.FlaggedForDeletion:
                case FileCrawlOptions.FlaggedForRescan:
                    dbEntry.Entity.Options = FileCrawlOptions.None;
                    break;
            }
            Guid id = dbEntry.Entity.Id;
            cancellationToken.ThrowIfCancellationRequested();
            await dbContext.SaveChangesAsync(cancellationToken);
            bool shouldSaveChanges = oldRedundantSet.ExistsInDb() && !(await oldRedundantSet.GetRelatedCollectionAsync(r => r.Redundancies, cancellationToken)).Any(r => r.FileId != id);
            if (shouldSaveChanges)
                dbContext.RedundantSets.Remove(oldRedundantSet.Entity);
            if (oldBinaryPropertySet.ExistsInDb() && oldBinaryPropertySet.Entity.Hash.HasValue || oldBinaryPropertySet.Entity.Length > 0 &&
                !(await oldBinaryPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.BinaryPropertySets.Remove(oldBinaryPropertySet.Entity);
            }
            if (oldSummaryPropertySet.ExistsInDb() && (await oldSummaryPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.SummaryPropertySets.Remove(oldSummaryPropertySet.Entity);
            }
            if (oldAudioPropertySet.ExistsInDb() && (await oldAudioPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.AudioPropertySets.Remove(oldAudioPropertySet.Entity);
            }
            if (oldDocumentPropertySet.ExistsInDb() && (await oldDocumentPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.DocumentPropertySets.Remove(oldDocumentPropertySet.Entity);
            }
            if (oldDRMPropertySet.ExistsInDb() && (await oldDRMPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.DRMPropertySets.Remove(oldDRMPropertySet.Entity);
            }
            if (oldGPSPropertySet.ExistsInDb() && (await oldGPSPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.GPSPropertySets.Remove(oldGPSPropertySet.Entity);
            }
            if (oldImagePropertySet.ExistsInDb() && (await oldImagePropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.ImagePropertySets.Remove(oldImagePropertySet.Entity);
            }
            if (oldMediaPropertySet.ExistsInDb() && (await oldMediaPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.MediaPropertySets.Remove(oldMediaPropertySet.Entity);
            }
            if (oldMusicPropertySet.ExistsInDb() && (await oldMusicPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.MusicPropertySets.Remove(oldMusicPropertySet.Entity);
            }
            if (oldPhotoPropertySet.ExistsInDb() && (await oldPhotoPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.PhotoPropertySets.Remove(oldPhotoPropertySet.Entity);
            }
            if (oldRecordedTVPropertySet.ExistsInDb() && (await oldRecordedTVPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.RecordedTVPropertySets.Remove(oldRecordedTVPropertySet.Entity);
            }
            if (oldVideoPropertySet.ExistsInDb() && (await oldVideoPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.VideoPropertySets.Remove(oldVideoPropertySet.Entity);
            }
            cancellationToken.ThrowIfCancellationRequested();
            if (shouldSaveChanges)
                await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetStatusDeleted(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<DbFile> dbEntry = dbContext.Entry(this);
            switch (dbEntry.State)
            {
                case EntityState.Detached:
                case EntityState.Deleted:
                    throw new InvalidOperationException();
            }
            if (Status == FileCorrelationStatus.Deleted)
                return;
            if (dbContext.Database.CurrentTransaction is null)
            {
                IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await SetStatusDeleted(dbEntry, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await transaction.CommitAsync(cancellationToken);
            }
            else
                await SetStatusDeleted(dbEntry, cancellationToken);
        }

        private static async Task<bool> ExpungeAsync(EntityEntry<DbFile> dbEntry, CancellationToken cancellationToken)
        {
            if (dbEntry.Context is not LocalDbContext dbContext)
                throw new InvalidOperationException();
            cancellationToken.ThrowIfCancellationRequested();
            Redundancy oldRedundancy = await dbEntry.GetRelatedReferenceAsync(f => f.Redundancy, cancellationToken);
            EntityEntry<RedundantSet> oldRedundantSet;
            if (oldRedundancy is null)
                oldRedundantSet = null;
            else
            {
                oldRedundantSet = await dbContext.Entry(oldRedundancy).GetRelatedTargetEntryAsync(r => r.RedundantSet, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                dbContext.Redundancies.Remove(oldRedundancy);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            EntityEntry<BinaryPropertySet> oldBinaryPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.BinaryProperties, cancellationToken);
            FileAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (accessErrors.Length > 0)
                dbContext.FileAccessErrors.RemoveRange(accessErrors);
            FileComparison[] comparisons = (await dbEntry.GetRelatedCollectionAsync(f => f.BaselineComparisons, cancellationToken))
                .Concat(await dbEntry.GetRelatedCollectionAsync(f => f.CorrelativeComparisons, cancellationToken)).ToArray();
            cancellationToken.ThrowIfCancellationRequested();
            if (comparisons.Length > 0)
                dbContext.Comparisons.RemoveRange(comparisons);
            EntityEntry<SummaryPropertySet> oldSummaryPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.SummaryProperties, cancellationToken);
            EntityEntry<AudioPropertySet> oldAudioPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.AudioProperties, cancellationToken);
            EntityEntry<DocumentPropertySet> oldDocumentPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.DocumentProperties, cancellationToken);
            EntityEntry<DRMPropertySet> oldDRMPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.DRMProperties, cancellationToken);
            EntityEntry<GPSPropertySet> oldGPSPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.GPSProperties, cancellationToken);
            EntityEntry<ImagePropertySet> oldImagePropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.ImageProperties, cancellationToken);
            EntityEntry<MediaPropertySet> oldMediaPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.MediaProperties, cancellationToken);
            EntityEntry<MusicPropertySet> oldMusicPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.MusicProperties, cancellationToken);
            EntityEntry<PhotoPropertySet> oldPhotoPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.PhotoProperties, cancellationToken);
            EntityEntry<RecordedTVPropertySet> oldRecordedTVPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.RecordedTVProperties, cancellationToken);
            EntityEntry<VideoPropertySet> oldVideoPropertySet = await dbEntry.GetRelatedTargetEntryAsync(f => f.VideoProperties, cancellationToken);
            Guid id = dbEntry.Entity.Id;
            cancellationToken.ThrowIfCancellationRequested();
            await dbContext.SaveChangesAsync(cancellationToken);
            bool shouldSaveChanges = oldRedundantSet.ExistsInDb() && !(await oldRedundantSet.GetRelatedCollectionAsync(r => r.Redundancies, cancellationToken)).Any(r => r.FileId != id);
            if (shouldSaveChanges)
                dbContext.RedundantSets.Remove(oldRedundantSet.Entity);
            if (oldBinaryPropertySet.ExistsInDb() && !(await oldBinaryPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.BinaryPropertySets.Remove(oldBinaryPropertySet.Entity);
            }
            if (oldSummaryPropertySet.ExistsInDb() && (await oldSummaryPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.SummaryPropertySets.Remove(oldSummaryPropertySet.Entity);
            }
            if (oldAudioPropertySet.ExistsInDb() && (await oldAudioPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.AudioPropertySets.Remove(oldAudioPropertySet.Entity);
            }
            if (oldDocumentPropertySet.ExistsInDb() && (await oldDocumentPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.DocumentPropertySets.Remove(oldDocumentPropertySet.Entity);
            }
            if (oldDRMPropertySet.ExistsInDb() && (await oldDRMPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.DRMPropertySets.Remove(oldDRMPropertySet.Entity);
            }
            if (oldGPSPropertySet.ExistsInDb() && (await oldGPSPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.GPSPropertySets.Remove(oldGPSPropertySet.Entity);
            }
            if (oldImagePropertySet.ExistsInDb() && (await oldImagePropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.ImagePropertySets.Remove(oldImagePropertySet.Entity);
            }
            if (oldMediaPropertySet.ExistsInDb() && (await oldMediaPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.MediaPropertySets.Remove(oldMediaPropertySet.Entity);
            }
            if (oldMusicPropertySet.ExistsInDb() && (await oldMusicPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.MusicPropertySets.Remove(oldMusicPropertySet.Entity);
            }
            if (oldPhotoPropertySet.ExistsInDb() && (await oldPhotoPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.PhotoPropertySets.Remove(oldPhotoPropertySet.Entity);
            }
            if (oldRecordedTVPropertySet.ExistsInDb() && (await oldRecordedTVPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.RecordedTVPropertySets.Remove(oldRecordedTVPropertySet.Entity);
            }
            if (oldVideoPropertySet.ExistsInDb() && (await oldVideoPropertySet.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).Any(f => f.Id != id))
            {
                shouldSaveChanges = true;
                dbContext.VideoPropertySets.Remove(oldVideoPropertySet.Entity);
            }
            cancellationToken.ThrowIfCancellationRequested();
            if (shouldSaveChanges)
                await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
