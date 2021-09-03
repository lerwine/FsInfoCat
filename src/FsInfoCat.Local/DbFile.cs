using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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

        public static async Task<int> DeleteAsync([DisallowNull] DbFile target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IStatusListener statusListener, string parentPath = null)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (statusListener is null)
                throw new ArgumentNullException(nameof(statusListener));
            string path = string.IsNullOrEmpty(parentPath) ? target.Name : Path.Combine(parentPath, target.Name);
            using IDisposable loggerScope = statusListener.Logger.BeginScope(target.Id);
            statusListener.Logger.LogDebug("Removing dependant records for DbFile {{ Id = {Id}; Path = \"{Path}\" }}", target.Id, path);
            statusListener.SetMessage($"Removing file record: {path}");
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            EntityEntry<DbFile> entry = dbContext.Entry(target);
            EntityEntry<BinaryPropertySet> binaryProperties = await entry.GetRelatedTargetEntryAsync(e => e.BinaryProperties, statusListener.CancellationToken);
            EntityEntry<SummaryPropertySet> summaryProperties = await entry.GetRelatedTargetEntryAsync(e => e.SummaryProperties, statusListener.CancellationToken);
            EntityEntry<DocumentPropertySet> documentProperties = await entry.GetRelatedTargetEntryAsync(e => e.DocumentProperties, statusListener.CancellationToken);
            EntityEntry<AudioPropertySet> audioProperties = await entry.GetRelatedTargetEntryAsync(e => e.AudioProperties, statusListener.CancellationToken);
            EntityEntry<DRMPropertySet> drmProperties = await entry.GetRelatedTargetEntryAsync(e => e.DRMProperties, statusListener.CancellationToken);
            EntityEntry<GPSPropertySet> gpsProperties = await entry.GetRelatedTargetEntryAsync(e => e.GPSProperties, statusListener.CancellationToken);
            EntityEntry<ImagePropertySet> imageProperties = await entry.GetRelatedTargetEntryAsync(e => e.ImageProperties, statusListener.CancellationToken);
            EntityEntry<MediaPropertySet> mediaProperties = await entry.GetRelatedTargetEntryAsync(e => e.MediaProperties, statusListener.CancellationToken);
            EntityEntry<MusicPropertySet> musicProperties = await entry.GetRelatedTargetEntryAsync(e => e.MusicProperties, statusListener.CancellationToken);
            EntityEntry<PhotoPropertySet> photoProperties = await entry.GetRelatedTargetEntryAsync(e => e.PhotoProperties, statusListener.CancellationToken);
            EntityEntry<RecordedTVPropertySet> recordedTVProperties = await entry.GetRelatedTargetEntryAsync(e => e.RecordedTVProperties, statusListener.CancellationToken);
            EntityEntry<VideoPropertySet> videoProperties = await entry.GetRelatedTargetEntryAsync(e => e.VideoProperties, statusListener.CancellationToken);
            Redundancy redundancy = await entry.GetRelatedReferenceAsync(e => e.Redundancy, statusListener.CancellationToken);
            int result = (redundancy is null) ? 0 : await Redundancy.DeleteAsync(redundancy, dbContext, statusListener.CancellationToken);
            FileComparison[] fileComparisons = (await entry.GetRelatedCollectionAsync(e => e.BaselineComparisons, statusListener.CancellationToken))
                .Concat(await entry.GetRelatedCollectionAsync(e => e.BaselineComparisons, statusListener.CancellationToken)).ToArray();
            if (fileComparisons.Length > 0)
                dbContext.Comparisons.RemoveRange(fileComparisons);
            FileAccessError[] accessErrors = (await entry.GetRelatedCollectionAsync(e => e.AccessErrors, statusListener.CancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.FileAccessErrors.RemoveRange(accessErrors);
            PersonalFileTag[] personalFileTags = (await entry.GetRelatedCollectionAsync(e => e.PersonalTags, statusListener.CancellationToken)).ToArray();
            if (personalFileTags.Length > 0)
                dbContext.PersonalFileTags.RemoveRange(personalFileTags);
            SharedFileTag[] sharedFileTags = (await entry.GetRelatedCollectionAsync(e => e.SharedTags, statusListener.CancellationToken)).ToArray();
            if (sharedFileTags.Length > 0)
                dbContext.SharedFileTags.RemoveRange(sharedFileTags);
            if (dbContext.ChangeTracker.HasChanges())
                result += await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            statusListener.Logger.LogInformation("Removing DbFile {{ Id = {Id}; Path = \"{Path}\" }}", target.Id, path);
            dbContext.Files.Remove(target);
            result += await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            statusListener.Logger.LogDebug("Removing unused former dependencies of DbFile {{ Id = {Id}; Path = \"{Path}\" }}", target.Id, path);
            if ((await binaryProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.BinaryPropertySets.Remove(binaryProperties.Entity);
            if (summaryProperties is not null && (await summaryProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.SummaryPropertySets.Remove(summaryProperties.Entity);
            if (documentProperties is not null && (await documentProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.DocumentPropertySets.Remove(documentProperties.Entity);
            if (audioProperties is not null && (await audioProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.AudioPropertySets.Remove(audioProperties.Entity);
            if (drmProperties is not null && (await drmProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.DRMPropertySets.Remove(drmProperties.Entity);
            if (gpsProperties is not null && (await gpsProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.GPSPropertySets.Remove(gpsProperties.Entity);
            if (imageProperties is not null && (await imageProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.ImagePropertySets.Remove(imageProperties.Entity);
            if (mediaProperties is not null && (await mediaProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.MediaPropertySets.Remove(mediaProperties.Entity);
            if (musicProperties is not null && (await musicProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.MusicPropertySets.Remove(musicProperties.Entity);
            if (photoProperties is not null && (await photoProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.PhotoPropertySets.Remove(photoProperties.Entity);
            if (recordedTVProperties is not null && (await recordedTVProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.RecordedTVPropertySets.Remove(recordedTVProperties.Entity);
            if (videoProperties is not null && (await videoProperties.GetRelatedCollectionAsync(p => p.Files, statusListener.CancellationToken)).Count() == 0)
                dbContext.VideoPropertySets.Remove(videoProperties.Entity);
            return dbContext.ChangeTracker.HasChanges() ? result + await dbContext.SaveChangesAsync(statusListener.CancellationToken) : result;
        }

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

        internal static void OnBuildEntity(EntityTypeBuilder<DbFile> builder)
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
            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
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
                _parent.SetValue(null);
        }

        protected override void RaiseBinaryPropertySetIdChanged(Guid value)
        {
            BinaryPropertySet nav = _binaryProperties.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _binaryProperties.SetValue(null);
        }

        protected override void OnSummaryPropertySetIdChanged(Guid? value)
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

        protected override void OnDocumentPropertySetIdChanged(Guid? value)
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

        protected override void OnAudioPropertySetIdChanged(Guid? value)
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

        protected override void OnDRMPropertySetIdChanged(Guid? value)
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

        protected override void OnGPSPropertySetIdChanged(Guid? value)
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

        protected override void OnImagePropertySetIdChanged(Guid? value)
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

        protected override void OnMediaPropertySetIdChanged(Guid? value)
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

        protected override void OnMusicPropertySetIdChanged(Guid? value)
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

        protected override void OnPhotoPropertySetIdChanged(Guid? value)
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

        protected override void OnRecordedTVPropertySetIdChanged(Guid? value)
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

        protected override void OnVideoPropertySetIdChanged(Guid? value)
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
