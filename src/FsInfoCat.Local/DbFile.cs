using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class DbFile : DbFileRow, ILocalFile, ISimpleIdentityReference<DbFile>, IEquatable<DbFile>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        public const string TABLE_NAME = "Files";

        private Guid? _parentId;
        private Subdirectory _parent;
        private Guid? _binaryPropertySetId;
        private BinaryPropertySet _binaryProperties;
        private Guid? _summaryPropertySetId;
        private SummaryPropertySet _summaryProperties;
        private Guid? _documentPropertySetId;
        private DocumentPropertySet _documentProperties;
        private Guid? _audioPropertySetId;
        private AudioPropertySet _audioProperties;
        private Guid? _drmPropertySetId;
        private DRMPropertySet _drmProperties;
        private Guid? _gpsPropertySetId;
        private GPSPropertySet _gpsProperties;
        private Guid? _imagePropertySetId;
        private ImagePropertySet _imageProperties;
        private Guid? _mediaPropertySetId;
        private MediaPropertySet _mediaProperties;
        private Guid? _musicPropertySetId;
        private MusicPropertySet _musicProperties;
        private Guid? _photoPropertySetId;
        private PhotoPropertySet _photoProperties;
        private Guid? _recordedTVPropertySetId;
        private RecordedTVPropertySet _recordedTVProperties;
        private Guid? _videoPropertySetId;
        private VideoPropertySet _videoProperties;
        private HashSet<FileAccessError> _accessErrors = new();
        private HashSet<FileComparison> _baselineComparisons = new();
        private HashSet<FileComparison> _correlativeComparisons = new();
        private HashSet<PersonalFileTag> _personalTags = new();
        private HashSet<SharedFileTag> _sharedTags = new();

        #endregion

        #region Properties

        public override Guid BinaryPropertySetId
        {
            get => _binaryProperties?.Id ?? _binaryPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_binaryProperties is not null)
                    {
                        if (_binaryProperties.Id.Equals(value)) return;
                        _binaryProperties = null;
                    }
                    _binaryPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_binaryProperties))]
        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _binaryProperties is not null && ReferenceEquals(value, _binaryProperties)) return;
                    _binaryPropertySetId = null;
                    _binaryProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid ParentId
        {
            get => _parent?.Id ?? _parentId ?? Guid.Empty;
            set
            {
                if (_parent is not null)
                {
                    if (_parent.Id.Equals(value)) return;
                    _parent = null;
                }
                _parentId = value;
            }
        }

        [BackingField(nameof(_parent))]
        public virtual Subdirectory Parent
        {
            get => _parent;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _parent is not null && ReferenceEquals(value, _parent)) return;
                    _parentId = null;
                    _parent = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public virtual Redundancy Redundancy { get; set; }

        public override Guid? SummaryPropertySetId
        {
            get => _summaryProperties?.Id ?? _summaryPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_summaryProperties is not null)
                    {
                        if (_summaryProperties.Id.Equals(value)) return;
                        _summaryProperties = null;
                    }
                    _summaryPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_summaryProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual SummaryPropertySet SummaryProperties
        {
            get => _summaryProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _summaryProperties is not null && ReferenceEquals(value, _summaryProperties)) return;
                    _summaryPropertySetId = null;
                    _summaryProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? DocumentPropertySetId
        {
            get => _documentProperties?.Id ?? _documentPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_documentProperties is not null)
                    {
                        if (_documentProperties.Id.Equals(value)) return;
                        _documentProperties = null;
                    }
                    _documentPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_documentProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DocumentPropertySet DocumentProperties
        {
            get => _documentProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _documentProperties is not null && ReferenceEquals(value, _documentProperties)) return;
                    _documentPropertySetId = null;
                    _documentProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? AudioPropertySetId
        {
            get => _audioProperties?.Id ?? _audioPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_audioProperties is not null)
                    {
                        if (_audioProperties.Id.Equals(value)) return;
                        _audioProperties = null;
                    }
                    _audioPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_audioProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual AudioPropertySet AudioProperties
        {
            get => _audioProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _audioProperties is not null && ReferenceEquals(value, _audioProperties)) return;
                    _audioPropertySetId = null;
                    _audioProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? DRMPropertySetId
        {
            get => _drmProperties?.Id ?? _drmPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_drmProperties is not null)
                    {
                        if (_drmProperties.Id.Equals(value)) return;
                        _drmProperties = null;
                    }
                    _drmPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_drmProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DRMPropertySet DRMProperties
        {
            get => _drmProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _drmProperties is not null && ReferenceEquals(value, _drmProperties)) return;
                    _drmPropertySetId = null;
                    _drmProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? GPSPropertySetId
        {
            get => _gpsProperties?.Id ?? _gpsPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_gpsProperties is not null)
                    {
                        if (_gpsProperties.Id.Equals(value)) return;
                        _gpsProperties = null;
                    }
                    _gpsPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_gpsProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual GPSPropertySet GPSProperties
        {
            get => _gpsProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _gpsProperties is not null && ReferenceEquals(value, _gpsProperties)) return;
                    _gpsPropertySetId = null;
                    _gpsProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? ImagePropertySetId
        {
            get => _imageProperties?.Id ?? _imagePropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_imageProperties is not null)
                    {
                        if (_imageProperties.Id.Equals(value)) return;
                        _imageProperties = null;
                    }
                    _imagePropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_imageProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ImagePropertySet ImageProperties
        {
            get => _imageProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _imageProperties is not null && ReferenceEquals(value, _imageProperties)) return;
                    _imagePropertySetId = null;
                    _imageProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? MediaPropertySetId
        {
            get => _mediaProperties?.Id ?? _mediaPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_mediaProperties is not null)
                    {
                        if (_mediaProperties.Id.Equals(value)) return;
                        _mediaProperties = null;
                    }
                    _mediaPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_mediaProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MediaPropertySet MediaProperties
        {
            get => _mediaProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _mediaProperties is not null && ReferenceEquals(value, _mediaProperties)) return;
                    _mediaPropertySetId = null;
                    _mediaProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? MusicPropertySetId
        {
            get => _musicProperties?.Id ?? _musicPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_musicProperties is not null)
                    {
                        if (_musicProperties.Id.Equals(value)) return;
                        _musicProperties = null;
                    }
                    _musicPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_musicProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MusicPropertySet MusicProperties
        {
            get => _musicProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _musicProperties is not null && ReferenceEquals(value, _musicProperties)) return;
                    _musicPropertySetId = null;
                    _musicProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? PhotoPropertySetId
        {
            get => _photoProperties?.Id ?? _photoPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_photoProperties is not null)
                    {
                        if (_photoProperties.Id.Equals(value)) return;
                        _photoProperties = null;
                    }
                    _photoProperties = null;
                    _photoPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }


        [BackingField(nameof(_photoProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual PhotoPropertySet PhotoProperties
        {
            get => _photoProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _photoProperties is not null && ReferenceEquals(value, _photoProperties)) return;
                    _photoPropertySetId = null;
                    _photoProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? RecordedTVPropertySetId
        {
            get => _recordedTVProperties?.Id ?? _recordedTVPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_recordedTVProperties is not null)
                    {
                        if (_recordedTVProperties.Id.Equals(value)) return;
                        _recordedTVProperties = null;
                    }
                    _recordedTVPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_recordedTVProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RecordedTVPropertySet RecordedTVProperties
        {
            get => _recordedTVProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _recordedTVProperties is not null && ReferenceEquals(value, _recordedTVProperties)) return;
                    _recordedTVPropertySetId = null;
                    _recordedTVProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? VideoPropertySetId
        {
            get => _videoProperties?.Id ?? _videoPropertySetId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_videoProperties is not null)
                    {
                        if (_videoProperties.Id.Equals(value)) return;
                        _videoProperties = null;
                    }
                    _videoPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_videoProperties))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VideoPropertySet VideoProperties
        {
            get => _videoProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _videoProperties is not null && ReferenceEquals(value, _videoProperties)) return;
                    _videoPropertySetId = null;
                    _videoProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [BackingField(nameof(_accessErrors))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        public virtual HashSet<FileAccessError> AccessErrors { get => _accessErrors; set => _accessErrors = value ?? new(); }

        [BackingField(nameof(_baselineComparisons))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        public virtual HashSet<FileComparison> BaselineComparisons { get => _baselineComparisons; set => _baselineComparisons = value ?? new(); }

        [BackingField(nameof(_correlativeComparisons))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        public virtual HashSet<FileComparison> CorrelativeComparisons { get => _correlativeComparisons; set => _correlativeComparisons = value ?? new(); }

        [BackingField(nameof(_personalTags))]
        [NotNull]
        public HashSet<PersonalFileTag> PersonalTags { get => _personalTags; set => _personalTags = value ?? new(); }

        [BackingField(nameof(_sharedTags))]
        [NotNull]
        public HashSet<SharedFileTag> SharedTags { get => _sharedTags; set => _sharedTags = value ?? new(); }

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
        IEnumerable<ILocalPersonalFileTag> ILocalFile.PersonalTags => SharedTags.Cast<ILocalPersonalFileTag>();
        IEnumerable<ILocalPersonalTag> ILocalDbFsItem.PersonalTags => SharedTags.Cast<ILocalPersonalTag>();
        IEnumerable<IPersonalFileTag> IFile.PersonalTags => SharedTags.Cast<IPersonalFileTag>();
        IEnumerable<IPersonalTag> IDbFsItem.PersonalTags => SharedTags.Cast<IPersonalTag>();
        IEnumerable<ILocalSharedFileTag> ILocalFile.SharedTags => SharedTags.Cast<ILocalSharedFileTag>();
        IEnumerable<ILocalSharedTag> ILocalDbFsItem.SharedTags => SharedTags.Cast<ILocalSharedTag>();
        IEnumerable<ISharedFileTag> IFile.SharedTags => SharedTags.Cast<ISharedFileTag>();
        IEnumerable<ISharedTag> IDbFsItem.SharedTags => SharedTags.Cast<ISharedTag>();
        DbFile IIdentityReference<DbFile>.Entity => this;

        #endregion

        public static async Task<bool> DeleteAsync([DisallowNull] DbFile target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] CancellationToken cancellationToken, ItemDeletionOption deletionOption = ItemDeletionOption.Default)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            EntityEntry<DbFile> entry = dbContext.Entry(target);
            if (!entry.ExistsInDb())
                return false;
            bool shouldDelete;
            switch (deletionOption)
            {
                case ItemDeletionOption.Default:
                    if (target.Options.HasFlag(FileCrawlOptions.FlaggedForDeletion))
                    {
                        if (target.Status == FileCorrelationStatus.Deleted)
                            return false;
                        target.Status = FileCorrelationStatus.Deleted;
                        shouldDelete = false;
                    }
                    else
                        shouldDelete = true;
                    break;
                case ItemDeletionOption.MarkAsDeleted:
                    if (target.Status == FileCorrelationStatus.Deleted)
                        return false;
                    shouldDelete = false;
                    target.Status = FileCorrelationStatus.Deleted;
                    break;
                default:
                    shouldDelete = true;
                    break;
            }
            await entry.RemoveRelatedEntitiesAsync(e => e.PersonalTags, dbContext.PersonalFileTags, cancellationToken);
            await entry.RemoveRelatedEntitiesAsync(e => e.SharedTags, dbContext.SharedFileTags, cancellationToken);
            await entry.RemoveRelatedEntitiesAsync(e => e.BaselineComparisons, dbContext.Comparisons, cancellationToken);
            await entry.RemoveRelatedEntitiesAsync(e => e.CorrelativeComparisons, dbContext.Comparisons, cancellationToken);
            await entry.RemoveRelatedEntitiesAsync(e => e.AccessErrors, dbContext.FileAccessErrors, cancellationToken);
            Guid id = target.Id;
            EntityEntry<SummaryPropertySet> oldSummaryProperties = await entry.GetRelatedTargetEntryAsync(e => e.SummaryProperties, cancellationToken);
            if (oldSummaryProperties is not null)
            {
                DbFile item = (await oldSummaryProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldSummaryProperties.Entity.Files.Count > 1)
                    oldSummaryProperties = null;
            }
            EntityEntry<DocumentPropertySet> oldDocumentProperties = await entry.GetRelatedTargetEntryAsync(e => e.DocumentProperties, cancellationToken);
            if (oldDocumentProperties is not null)
            {
                DbFile item = (await oldDocumentProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldDocumentProperties.Entity.Files.Count > 1)
                    oldDocumentProperties = null;
            }
            EntityEntry<AudioPropertySet> oldAudioProperties = await entry.GetRelatedTargetEntryAsync(e => e.AudioProperties, cancellationToken);
            if (oldAudioProperties is not null)
            {
                DbFile item = (await oldAudioProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldAudioProperties.Entity.Files.Count > 1)
                    oldAudioProperties = null;
            }
            EntityEntry<DRMPropertySet> oldDRMProperties = await entry.GetRelatedTargetEntryAsync(e => e.DRMProperties, cancellationToken);
            if (oldDRMProperties is not null)
            {
                DbFile item = (await oldDRMProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldDRMProperties.Entity.Files.Count > 1)
                    oldDRMProperties = null;
            }
            EntityEntry<GPSPropertySet> oldGPSProperties = await entry.GetRelatedTargetEntryAsync(e => e.GPSProperties, cancellationToken);
            if (oldGPSProperties is not null)
            {
                DbFile item = (await oldGPSProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldGPSProperties.Entity.Files.Count > 1)
                    oldGPSProperties = null;
            }
            EntityEntry<ImagePropertySet> oldImageProperties = await entry.GetRelatedTargetEntryAsync(e => e.ImageProperties, cancellationToken);
            if (oldImageProperties is not null)
            {
                DbFile item = (await oldImageProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldImageProperties.Entity.Files.Count > 1)
                    oldImageProperties = null;
            }
            EntityEntry<MediaPropertySet> oldMediaProperties = await entry.GetRelatedTargetEntryAsync(e => e.MediaProperties, cancellationToken);
            if (oldMediaProperties is not null)
            {
                DbFile item = (await oldMediaProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldMediaProperties.Entity.Files.Count > 1)
                    oldMediaProperties = null;
            }
            EntityEntry<MusicPropertySet> oldMusicProperties = await entry.GetRelatedTargetEntryAsync(e => e.MusicProperties, cancellationToken);
            if (oldMusicProperties is not null)
            {
                DbFile item = (await oldMusicProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldMusicProperties.Entity.Files.Count > 1)
                    oldMusicProperties = null;
            }
            EntityEntry<PhotoPropertySet> oldPhotoProperties = await entry.GetRelatedTargetEntryAsync(e => e.PhotoProperties, cancellationToken);
            if (oldPhotoProperties is not null)
            {
                DbFile item = (await oldPhotoProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldPhotoProperties.Entity.Files.Count > 1)
                    oldPhotoProperties = null;
            }
            EntityEntry<RecordedTVPropertySet> oldRecordedTVProperties = await entry.GetRelatedTargetEntryAsync(e => e.RecordedTVProperties, cancellationToken);
            if (oldRecordedTVProperties is not null)
            {
                DbFile item = (await oldRecordedTVProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldRecordedTVProperties.Entity.Files.Count > 1)
                    oldRecordedTVProperties = null;
            }
            EntityEntry<VideoPropertySet> oldVideoProperties = await entry.GetRelatedTargetEntryAsync(e => e.VideoProperties, cancellationToken);
            if (oldVideoProperties is not null)
            {
                DbFile item = (await oldVideoProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldVideoProperties.Entity.Files.Count > 1)
                    oldVideoProperties = null;
            }
            EntityEntry<BinaryPropertySet> oldBinaryProperties = await entry.GetRelatedTargetEntryAsync(e => e.BinaryProperties, cancellationToken);
            EntityEntry<Redundancy> oldRedundancy = await entry.GetRelatedTargetEntryAsync(e => e.Redundancy, cancellationToken);
            if (!shouldDelete)
            {
                if (oldBinaryProperties.Entity.Length > 0L)
                {
                    BinaryPropertySet binaryPropertySet = await dbContext.BinaryPropertySets.FirstOrDefaultAsync(p => p.Length == 0);
                    RedundantSet redundantSet;
                    if (binaryPropertySet is null)
                    {
                        binaryPropertySet = new() { Length = 0L };
                        dbContext.BinaryPropertySets.Add(binaryPropertySet);
                        target.BinaryProperties = binaryPropertySet;
                        redundantSet = new()
                        {
                            BinaryProperties = binaryPropertySet,
                            Status = RedundancyRemediationStatus.Deleted,
                        };
                        dbContext.RedundantSets.Add(redundantSet);
                    }
                    else
                    {
                        redundantSet = (await dbContext.Entry(binaryPropertySet).GetRelatedCollectionAsync(b => b.RedundantSets, cancellationToken)).FirstOrDefault(r => r.Status == RedundancyRemediationStatus.Deleted);
                        if (redundantSet is null)
                        {
                            redundantSet = new()
                            {
                                BinaryProperties = binaryPropertySet,
                                Status = RedundancyRemediationStatus.Deleted,
                            };
                            dbContext.RedundantSets.Add(redundantSet);
                        }
                    }
                    Redundancy redundancy = new()
                    {
                        File = target,
                        RedundantSet = redundantSet
                    };
                    dbContext.Redundancies.Add(redundancy);
                    target.Redundancy = redundancy;
                }
                else
                {
                    oldBinaryProperties = null;
                    RedundantSet redundantSet = await oldRedundancy.GetRelatedReferenceAsync(r => r.RedundantSet, cancellationToken);
                    if (redundantSet.Status != RedundancyRemediationStatus.Deleted)
                    {
                        oldRedundancy = null;
                        EntityEntry<BinaryPropertySet> binaryPropertySet = await dbContext.Entry(redundantSet).GetRelatedTargetEntryAsync(r => r.BinaryProperties, cancellationToken);
                        redundantSet = (await binaryPropertySet.GetRelatedCollectionAsync(b => b.RedundantSets, cancellationToken)).FirstOrDefault(r => r.Status == RedundancyRemediationStatus.Deleted);
                        if (redundantSet is null)
                        {
                            redundantSet = new()
                            {
                                BinaryProperties = binaryPropertySet.Entity,
                                Status = RedundancyRemediationStatus.Deleted,
                            };
                            dbContext.RedundantSets.Add(redundantSet);
                        }
                        Redundancy redundancy = new()
                        {
                            File = target,
                            RedundantSet = redundantSet
                        };
                        dbContext.Redundancies.Add(redundancy);
                        target.Redundancy = redundancy;
                    }
                }
            }
            if (oldBinaryProperties is not null)
            {
                DbFile item = (await oldBinaryProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldBinaryProperties.Entity.Files.Count > 1)
                    oldBinaryProperties = null;
            }
            if (oldRedundancy is not null)
            {
                EntityEntry<RedundantSet> oldRedundantSet = await oldRedundancy.GetRelatedTargetEntryAsync(r => r.RedundantSet, cancellationToken);
                if ((await oldRedundantSet.GetRelatedCollectionAsync(r => r.Redundancies, cancellationToken)).Any(r => r.FileId != id))
                    oldRedundancy = null;
            }
            if (oldRedundancy is not null)
                dbContext.Redundancies.Remove(oldRedundancy.Entity);
            if (shouldDelete)
                dbContext.Remove(target);
            if (oldBinaryProperties is not null)
                dbContext.BinaryPropertySets.Remove(oldBinaryProperties.Entity);
            if (oldSummaryProperties is not null)
                dbContext.SummaryPropertySets.Remove(oldSummaryProperties.Entity);
            if (oldDocumentProperties is not null)
                dbContext.DocumentPropertySets.Remove(oldDocumentProperties.Entity);
            if (oldAudioProperties is not null)
                dbContext.AudioPropertySets.Remove(oldAudioProperties.Entity);
            if (oldDRMProperties is not null)
                dbContext.DRMPropertySets.Remove(oldDRMProperties.Entity);
            if (oldGPSProperties is not null)
                dbContext.GPSPropertySets.Remove(oldGPSProperties.Entity);
            if (oldImageProperties is not null)
                dbContext.ImagePropertySets.Remove(oldImageProperties.Entity);
            if (oldMediaProperties is not null)
                dbContext.MediaPropertySets.Remove(oldMediaProperties.Entity);
            if (oldMusicProperties is not null)
                dbContext.MusicPropertySets.Remove(oldMusicProperties.Entity);
            if (oldPhotoProperties is not null)
                dbContext.PhotoPropertySets.Remove(oldPhotoProperties.Entity);
            if (oldRecordedTVProperties is not null)
                dbContext.RecordedTVPropertySets.Remove(oldRecordedTVProperties.Entity);
            if (oldVideoProperties is not null)
                dbContext.VideoPropertySets.Remove(oldVideoProperties.Entity);
            return shouldDelete;
        }

        internal static void OnBuildEntity(EntityTypeBuilder<DbFile> builder)
        {
            _ = builder.HasOne(sn => sn.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.BinaryProperties).WithMany(d => d.Files).HasForeignKey(nameof(BinaryPropertySetId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.SummaryProperties).WithMany(d => d.Files).HasForeignKey(nameof(SummaryPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.DocumentProperties).WithMany(d => d.Files).HasForeignKey(nameof(DocumentPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.AudioProperties).WithMany(d => d.Files).HasForeignKey(nameof(AudioPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.DRMProperties).WithMany(d => d.Files).HasForeignKey(nameof(DRMPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.GPSProperties).WithMany(d => d.Files).HasForeignKey(nameof(GPSPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.ImageProperties).WithMany(d => d.Files).HasForeignKey(nameof(ImagePropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.MediaProperties).WithMany(d => d.Files).HasForeignKey(nameof(MediaPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.MusicProperties).WithMany(d => d.Files).HasForeignKey(nameof(MusicPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.PhotoProperties).WithMany(d => d.Files).HasForeignKey(nameof(PhotoPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.RecordedTVProperties).WithMany(d => d.Files).HasForeignKey(nameof(RecordedTVPropertySetId)).OnDelete(DeleteBehavior.Restrict);
            _ = builder.HasOne(sn => sn.VideoProperties).WithMany(d => d.Files).HasForeignKey(nameof(VideoPropertySetId)).OnDelete(DeleteBehavior.Restrict);
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
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            if (string.IsNullOrEmpty(name) || (entry = validationContext.GetService<EntityEntry>()) is null ||
                (dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>()) is null)
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
            //DateTime oldCreationTime = CreationTime;
            //DateTime oldLastWriteTime = LastWriteTime;
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
                _ = await dbContext.SaveChangesAsync(cancellationToken);
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
                _ = await dbContext.SaveChangesAsync(cancellationToken);
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
            _ = await dbContext.SaveChangesAsync(cancellationToken);
            // TODO: Need to add code later to see where comparisons need to be made
            return entry;
        }

        IEnumerable<Guid> IIdentityReference.GetIdentifiers() { yield return Id; }

        public ISimpleIdentityReference<BinaryPropertySet> GetBinaryPropertySetReference()
        {
            BinaryPropertySet binaryPropertySet = BinaryProperties;
            return (binaryPropertySet is null) ? (TryGetId(out Guid id) ? new SimpleIdentityOnlyReference<BinaryPropertySet>(id) : null) : new SimpleIdentityReference<BinaryPropertySet>(binaryPropertySet, b => b.Id);
        }

        public bool Equals(DbFile other) => other is not null && (ReferenceEquals(this, other) || (TryGetId(out Guid id) ? id.Equals(other.Id) : other.Id.Equals(Guid.Empty) && ArePropertiesEqual(this)));

        public override bool Equals(DbFileRow other) => other is not null && ((other is DbFile file) ? Equals(file) : base.Equals(other));

        public bool Equals(IFile other)
        {
            if (other is null) return false;
            if (other is DbFile dbFile) return Equals(dbFile);
            return TryGetId(out Guid id) ? id.Equals(other.Id) : (other.Id.Equals(Guid.Empty) && ArePropertiesEqual(this));
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is DbFile dbFile) return Equals(dbFile);
            if (obj is not IFileRow row) return false;
            if (TryGetId(out Guid id)) return id.Equals(row.Id);
            if (row.Id.Equals(Guid.Empty)) return false;
            if (row is ILocalFileRow localRow) return ArePropertiesEqual(localRow);
            if (row is IFile file) return ArePropertiesEqual(file);
            return ArePropertiesEqual(row);
        }

        public bool TryGetBinaryPropertySetId(out Guid binaryPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_binaryProperties is null)
                {
                    if (_binaryPropertySetId.HasValue)
                    {
                        binaryPropertySetId = _binaryPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _binaryProperties.TryGetId(out binaryPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            binaryPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetSummaryPropertySetId(out Guid summaryPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_summaryProperties is null)
                {
                    if (_summaryPropertySetId.HasValue)
                    {
                        summaryPropertySetId = _summaryPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _summaryProperties.TryGetId(out summaryPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            summaryPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetDocumentPropertySetId(out Guid documentPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_documentProperties is null)
                {
                    if (_documentPropertySetId.HasValue)
                    {
                        documentPropertySetId = _documentPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _documentProperties.TryGetId(out documentPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            documentPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetAudioPropertySetId(out Guid audioPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_audioProperties is null)
                {
                    if (_audioPropertySetId.HasValue)
                    {
                        audioPropertySetId = _audioPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _audioProperties.TryGetId(out audioPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            audioPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetDRMPropertySetId(out Guid drmPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_drmProperties is null)
                {
                    if (_drmPropertySetId.HasValue)
                    {
                        drmPropertySetId = _drmPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _drmProperties.TryGetId(out drmPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            drmPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetGPSPropertySetId(out Guid gpsPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_gpsProperties is null)
                {
                    if (_gpsPropertySetId.HasValue)
                    {
                        gpsPropertySetId = _gpsPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _gpsProperties.TryGetId(out gpsPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            gpsPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetImagePropertySetId(out Guid imagePropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_imageProperties is null)
                {
                    if (_imagePropertySetId.HasValue)
                    {
                        imagePropertySetId = _imagePropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _imageProperties.TryGetId(out imagePropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            imagePropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetMediaPropertySetId(out Guid mediaPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_mediaProperties is null)
                {
                    if (_mediaPropertySetId.HasValue)
                    {
                        mediaPropertySetId = _mediaPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _mediaProperties.TryGetId(out mediaPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            mediaPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetMusicPropertySetId(out Guid musicPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_musicProperties is null)
                {
                    if (_musicPropertySetId.HasValue)
                    {
                        musicPropertySetId = _musicPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _musicProperties.TryGetId(out musicPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            musicPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetPhotoPropertySetId(out Guid photoPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_photoProperties is null)
                {
                    if (_photoPropertySetId.HasValue)
                    {
                        photoPropertySetId = _photoPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _photoProperties.TryGetId(out photoPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            photoPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetRecordedTVPropertySetId(out Guid recordedTVPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_recordedTVProperties is null)
                {
                    if (_recordedTVPropertySetId.HasValue)
                    {
                        recordedTVPropertySetId = _recordedTVPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _recordedTVProperties.TryGetId(out recordedTVPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            recordedTVPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetVideoPropertySetId(out Guid videoPropertySetId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_videoProperties is null)
                {
                    if (_videoPropertySetId.HasValue)
                    {
                        videoPropertySetId = _videoPropertySetId.Value;
                        return true;
                    }
                }
                else
                    return _videoProperties.TryGetId(out videoPropertySetId);
            }
            finally { Monitor.Exit(SyncRoot); }
            videoPropertySetId = Guid.Empty;
            return false;
        }

        public bool TryGetParentId(out Guid subdirectoryId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_parent is null)
                {
                    if (_parentId.HasValue)
                    {
                        subdirectoryId = _parentId.Value;
                        return true;
                    }
                }
                else
                    return _parent.TryGetId(out subdirectoryId);
            }
            finally { Monitor.Exit(SyncRoot); }
            subdirectoryId = Guid.Empty;
            return false;
        }
    }
}
