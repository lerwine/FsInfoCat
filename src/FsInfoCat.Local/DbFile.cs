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
    public class DbFile : DbFileRow, ILocalFile, ISimpleIdentityReference<DbFile>
    {
        #region Fields

        public const string TABLE_NAME = "Files";

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
        private HashSet<PersonalFileTag> _personalTags = new();
        private HashSet<SharedFileTag> _sharedTags = new();

        #endregion

        #region Properties

        public virtual BinaryPropertySet BinaryProperties
        {
            get => _binaryProperties.GetValue();
            set
            {
                if (_binaryProperties.SetValue(value))
                    BinaryPropertySetId = value?.Id ?? Guid.Empty;
            }
        }

        public virtual Subdirectory Parent
        {
            get => _parent.GetValue();
            set
            {
                if (_parent.SetValue(value))
                    ParentId = value?.Id ?? Guid.Empty;
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
                    SummaryPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DocumentProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DocumentPropertySet DocumentProperties
        {
            get => _documentProperties.GetValue();
            set
            {
                if (_documentProperties.SetValue(value))
                    DocumentPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AudioProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual AudioPropertySet AudioProperties
        {
            get => _audioProperties.GetValue();
            set
            {
                if (_audioProperties.SetValue(value))
                    AudioPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DRMProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DRMPropertySet DRMProperties
        {
            get => _drmProperties.GetValue();
            set
            {
                if (_drmProperties.SetValue(value))
                    DRMPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_GPSProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual GPSPropertySet GPSProperties
        {
            get => _gpsProperties.GetValue();
            set
            {
                if (_gpsProperties.SetValue(value))
                    GPSPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ImageProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ImagePropertySet ImageProperties
        {
            get => _imageProperties.GetValue();
            set
            {
                if (_imageProperties.SetValue(value))
                    ImagePropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MediaProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MediaPropertySet MediaProperties
        {
            get => _mediaProperties.GetValue();
            set
            {
                if (_mediaProperties.SetValue(value))
                    MediaPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MusicProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MusicPropertySet MusicProperties
        {
            get => _musicProperties.GetValue();
            set
            {
                if (_musicProperties.SetValue(value))
                    MusicPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_PhotoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual PhotoPropertySet PhotoProperties
        {
            get => _photoProperties.GetValue();
            set
            {
                if (_photoProperties.SetValue(value))
                    PhotoPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RecordedTVProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RecordedTVPropertySet RecordedTVProperties
        {
            get => _recordedTVProperties.GetValue();
            set
            {
                if (_recordedTVProperties.SetValue(value))
                    RecordedTVPropertySetId = value?.Id;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VideoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VideoPropertySet VideoProperties
        {
            get => _videoProperties.GetValue();
            set
            {
                if (_videoProperties.SetValue(value))
                    VideoPropertySetId = value?.Id;
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

        public HashSet<PersonalFileTag> PersonalTags
        {
            get => _personalTags;
            set => CheckHashSetChanged(_personalTags, value, h => _personalTags = h);
        }

        public HashSet<SharedFileTag> SharedTags
        {
            get => _sharedTags;
            set => CheckHashSetChanged(_sharedTags, value, h => _sharedTags = h);
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

        public DbFile()
        {
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
                case ItemDeletionOption.NarkAsDeleted:
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

        protected override void OnParentIdChanged(Guid value)
        {
            Subdirectory nav = _parent.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _ = _parent.SetValue(null);
        }

        protected override void RaiseBinaryPropertySetIdChanged(Guid value)
        {
            BinaryPropertySet nav = _binaryProperties.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnSummaryPropertySetIdChanged(Guid? value)
        {
            SummaryPropertySet nav = _summaryProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnDocumentPropertySetIdChanged(Guid? value)
        {
            DocumentPropertySet nav = _documentProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnAudioPropertySetIdChanged(Guid? value)
        {
            AudioPropertySet nav = _audioProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnDRMPropertySetIdChanged(Guid? value)
        {
            DRMPropertySet nav = _drmProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnGPSPropertySetIdChanged(Guid? value)
        {
            GPSPropertySet nav = _gpsProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnImagePropertySetIdChanged(Guid? value)
        {
            ImagePropertySet nav = _imageProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnMediaPropertySetIdChanged(Guid? value)
        {
            MediaPropertySet nav = _mediaProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnMusicPropertySetIdChanged(Guid? value)
        {
            MusicPropertySet nav = _musicProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnPhotoPropertySetIdChanged(Guid? value)
        {
            PhotoPropertySet nav = _photoProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnRecordedTVPropertySetIdChanged(Guid? value)
        {
            RecordedTVPropertySet nav = _recordedTVProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }

        protected override void OnVideoPropertySetIdChanged(Guid? value)
        {
            VideoPropertySet nav = _videoProperties.GetValue();
            if (value.HasValue)
            {
                if (!(nav is null || nav.Id.Equals(value.Value)))
                    _ = _binaryProperties.SetValue(null);
            }
            else if (!(nav is null))
                _ = _binaryProperties.SetValue(null);
        }
    }
}
