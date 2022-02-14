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
    public class DbFile : DbFileRow, ILocalFile, ISimpleIdentityReference<DbFile>, IEquatable<DbFile>
    {
        #region Fields

        public const string TABLE_NAME = "Files";

        private Subdirectory _parent;
        private BinaryPropertySet _binaryProperties;
        private SummaryPropertySet _summaryProperties;
        private DocumentPropertySet _documentProperties;
        private AudioPropertySet _audioProperties;
        private DRMPropertySet _drmProperties;
        private GPSPropertySet _gpsProperties;
        private ImagePropertySet _imageProperties;
        private MediaPropertySet _mediaProperties;
        private MusicPropertySet _musicProperties;
        private PhotoPropertySet _photoProperties;
        private RecordedTVPropertySet _recordedTVProperties;
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
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _binaryProperties?.Id;
                    if (id.HasValue && id.Value != base.BinaryPropertySetId)
                    {
                        base.BinaryPropertySetId = id.Value;
                        return id.Value;
                    }
                    return base.BinaryPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _binaryProperties?.Id;
                    if (id.HasValue && id.Value != value)
                        _binaryProperties = null;
                    base.BinaryPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_binaryProperties is not null)
                            base.BinaryPropertySetId = Guid.Empty;
                    }
                    else
                    {
                        base.BinaryPropertySetId = value.Id;
                        _binaryProperties = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid ParentId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _parent?.Id;
                    if (id.HasValue && id.Value != base.ParentId)
                    {
                        base.ParentId = id.Value;
                        return id.Value;
                    }
                    return base.ParentId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _parent?.Id;
                    if (id.HasValue && id.Value != value)
                        _parent = null;
                    base.ParentId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public virtual Subdirectory Parent
        {
            get => _parent;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_parent is not null)
                            base.ParentId = Guid.Empty;
                    }
                    else
                    {
                        base.ParentId = value.Id;
                        _parent = value;
                    }
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public virtual Redundancy Redundancy { get; set; }

        public override Guid? SummaryPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _summaryProperties?.Id;
                    if (id.HasValue && id != base.SummaryPropertySetId)
                    {
                        base.SummaryPropertySetId = id;
                        return id;
                    }
                    return base.SummaryPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _summaryProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _summaryProperties = null;
                    }
                    else
                        _summaryProperties = null;
                    base.SummaryPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_SummaryProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual SummaryPropertySet SummaryProperties
        {
            get => _summaryProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_summaryProperties is not null)
                            base.SummaryPropertySetId = null;
                    }
                    else
                        base.SummaryPropertySetId = value.Id;
                    _summaryProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? DocumentPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _documentProperties?.Id;
                    if (id.HasValue && id != base.DocumentPropertySetId)
                    {
                        base.DocumentPropertySetId = id;
                        return id;
                    }
                    return base.DocumentPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _documentProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _documentProperties = null;
                    }
                    else
                        _documentProperties = null;
                    base.DocumentPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DocumentPropertySet DocumentProperties
        {
            get => _documentProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_documentProperties is not null)
                            base.DocumentPropertySetId = null;
                    }
                    else
                        base.DocumentPropertySetId = value.Id;
                    _documentProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? AudioPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _audioProperties?.Id;
                    if (id.HasValue && id != base.AudioPropertySetId)
                    {
                        base.AudioPropertySetId = id;
                        return id;
                    }
                    return base.AudioPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _audioProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _audioProperties = null;
                    }
                    else
                        _audioProperties = null;
                    base.AudioPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual AudioPropertySet AudioProperties
        {
            get => _audioProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_audioProperties is not null)
                            base.AudioPropertySetId = null;
                    }
                    else
                        base.AudioPropertySetId = value.Id;
                    _audioProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? DRMPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _drmProperties?.Id;
                    if (id.HasValue && id != base.DRMPropertySetId)
                    {
                        base.DRMPropertySetId = id;
                        return id;
                    }
                    return base.DRMPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _drmProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _drmProperties = null;
                    }
                    else
                        _drmProperties = null;
                    base.DRMPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DRMPropertySet DRMProperties
        {
            get => _drmProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_drmProperties is not null)
                            base.DRMPropertySetId = null;
                    }
                    else
                        base.DRMPropertySetId = value.Id;
                    _drmProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? GPSPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _gpsProperties?.Id;
                    if (id.HasValue && id != base.GPSPropertySetId)
                    {
                        base.GPSPropertySetId = id;
                        return id;
                    }
                    return base.GPSPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _gpsProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _gpsProperties = null;
                    }
                    else
                        _gpsProperties = null;
                    base.GPSPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual GPSPropertySet GPSProperties
        {
            get => _gpsProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_gpsProperties is not null)
                            base.GPSPropertySetId = null;
                    }
                    else
                        base.GPSPropertySetId = value.Id;
                    _gpsProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? ImagePropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _imageProperties?.Id;
                    if (id.HasValue && id != base.ImagePropertySetId)
                    {
                        base.ImagePropertySetId = id;
                        return id;
                    }
                    return base.ImagePropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _imageProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _imageProperties = null;
                    }
                    else
                        _imageProperties = null;
                    base.ImagePropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ImagePropertySet ImageProperties
        {
            get => _imageProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_imageProperties is not null)
                            base.ImagePropertySetId = null;
                    }
                    else
                        base.ImagePropertySetId = value.Id;
                    _imageProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? MediaPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _mediaProperties?.Id;
                    if (id.HasValue && id != base.MediaPropertySetId)
                    {
                        base.MediaPropertySetId = id;
                        return id;
                    }
                    return base.MediaPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _mediaProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _mediaProperties = null;
                    }
                    else
                        _mediaProperties = null;
                    base.MediaPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MediaPropertySet MediaProperties
        {
            get => _mediaProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_mediaProperties is not null)
                            base.MediaPropertySetId = null;
                    }
                    else
                        base.MediaPropertySetId = value.Id;
                    _mediaProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? MusicPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _musicProperties?.Id;
                    if (id.HasValue && id != base.MusicPropertySetId)
                    {
                        base.MusicPropertySetId = id;
                        return id;
                    }
                    return base.MusicPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _musicProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _musicProperties = null;
                    }
                    else
                        _musicProperties = null;
                    base.MusicPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MusicPropertySet MusicProperties
        {
            get => _musicProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_musicProperties is not null)
                            base.MusicPropertySetId = null;
                    }
                    else
                        base.MusicPropertySetId = value.Id;
                    _musicProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? PhotoPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _photoProperties?.Id;
                    if (id.HasValue && id != base.PhotoPropertySetId)
                    {
                        base.PhotoPropertySetId = id;
                        return id;
                    }
                    return base.PhotoPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _photoProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _photoProperties = null;
                    }
                    else
                        _photoProperties = null;
                    base.PhotoPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual PhotoPropertySet PhotoProperties
        {
            get => _photoProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_photoProperties is not null)
                            base.PhotoPropertySetId = null;
                    }
                    else
                        base.PhotoPropertySetId = value.Id;
                    _photoProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? RecordedTVPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _recordedTVProperties?.Id;
                    if (id.HasValue && id != base.RecordedTVPropertySetId)
                    {
                        base.RecordedTVPropertySetId = id;
                        return id;
                    }
                    return base.RecordedTVPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _recordedTVProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _recordedTVProperties = null;
                    }
                    else
                        _recordedTVProperties = null;
                    base.RecordedTVPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RecordedTVPropertySet RecordedTVProperties
        {
            get => _recordedTVProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_recordedTVProperties is not null)
                            base.RecordedTVPropertySetId = null;
                    }
                    else
                        base.RecordedTVPropertySetId = value.Id;
                    _recordedTVProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        public override Guid? VideoPropertySetId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _videoProperties?.Id;
                    if (id.HasValue && id != base.VideoPropertySetId)
                    {
                        base.VideoPropertySetId = id;
                        return id;
                    }
                    return base.VideoPropertySetId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value.HasValue)
                    {
                        Guid? id = _videoProperties?.Id;
                        if (id.HasValue && value.HasValue && id.Value == value.Value)
                            _videoProperties = null;
                    }
                    else
                        _videoProperties = null;
                    base.VideoPropertySetId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VideoPropertySet VideoProperties
        {
            get => _videoProperties;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_videoProperties is not null)
                            base.VideoPropertySetId = null;
                    }
                    else
                        base.VideoPropertySetId = value.Id;
                    _videoProperties = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileAccessError> AccessErrors { get => _accessErrors; set => _accessErrors = value ?? new(); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_BaselineComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> BaselineComparisons { get => _baselineComparisons; set => _baselineComparisons = value ?? new(); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CorrelativeComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> CorrelativeComparisons { get => _correlativeComparisons; set => _correlativeComparisons = value ?? new(); }

        public HashSet<PersonalFileTag> PersonalTags { get => _personalTags; set => _personalTags = value ?? new(); }

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

        IEnumerable<Guid> IIdentityReference.GetIdentifiers()
        {
            yield return Id;
        }

        public ISimpleIdentityReference<BinaryPropertySet> GetBinaryPropertySetReference()
        {
            // TODO: Implement GetBinaryPropertySetReference()
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalFile other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IFile other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DbFile other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IFile other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
