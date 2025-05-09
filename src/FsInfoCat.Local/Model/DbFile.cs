using FsInfoCat.Model;
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

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Database entity that represents a structural instance of file on a local host file system.
    /// </summary>
    /// <seealso cref="FileWithAncestorNames" />
    /// <seealso cref="FileWithBinaryProperties" />
    /// <seealso cref="FileWithBinaryPropertiesAndAncestorNames" />
    /// <seealso cref="LocalDbContext.Files" />
    [Table(TABLE_NAME)]
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class DbFile : DbFileRow, ILocalFile, IEquatable<DbFile>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        /// <summary>
        /// The name of the database table for this entity.
        /// </summary>
        public const string TABLE_NAME = "Files";

        private readonly SubdirectoryReference _parent;
        private readonly BinaryPropertySetReference _binaryProperties;
        private readonly SummaryPropertySetReference _summaryProperties;
        private readonly DocumentPropertySetReference _documentProperties;
        private readonly AudioPropertySetReference _audioProperties;
        private readonly DRMPropertySetReference _drmProperties;
        private readonly GPSPropertySetReference _gpsProperties;
        private readonly ImagePropertySetReference _imageProperties;
        private readonly MediaPropertySetReference _mediaProperties;
        private readonly MusicPropertySetReference _musicProperties;
        private readonly PhotoPropertySetReference _photoProperties;
        private readonly RecordedTVPropertySetReference _recordedTVProperties;
        private readonly VideoPropertySetReference _videoProperties;
        private HashSet<FileAccessError> _accessErrors = [];
        private HashSet<FileComparison> _baselineComparisons = [];
        private HashSet<FileComparison> _correlativeComparisons = [];
        private HashSet<PersonalFileTag> _personalTags = [];
        private HashSet<SharedFileTag> _sharedTags = [];

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the parent subdirectory.
        /// </summary>
        /// <value>The <see cref="SubdirectoryRow.Id" /> of the parent <see cref="Subdirectory" /> entity.</value>
        public override Guid ParentId { get => _parent.Id; set => _parent.SetId(value); }

        /// <summary>
        /// Gets the parent subdirectory.
        /// </summary>
        /// <value>The parent <see cref="Subdirectory" /> .</value>
        public virtual Subdirectory Parent { get => _parent.Entity; set => _parent.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated binary properties entity.
        /// </summary>
        /// <value>The <see cref="BinaryPropertySet.Id" /> of the <see cref="BinaryPropertySet" /> that has the length and MD5 hash that matches the current file.</value>
        public override Guid BinaryPropertySetId { get => _binaryProperties.Id; set => _binaryProperties.SetId(value); }

        /// <summary>
        /// Gets the binary properties for the current file.
        /// </summary>
        /// <value>The <see cref="BinaryPropertySet" /> that contains the file size and optionally, the <see cref="MD5Hash">MD5 hash</see> value of its binary
        /// contents.</value>
        public virtual BinaryPropertySet BinaryProperties { get => _binaryProperties.Entity; set => _binaryProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated summary properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="SummaryPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no summary properties.</value>
        public override Guid? SummaryPropertySetId { get => _summaryProperties.IdValue; set => _summaryProperties.SetId(value); }

        /// <summary>
        /// Gets the summary properties for the current file.
        /// </summary>
        /// <value>The <see cref="SummaryPropertySet" /> that contains the summary properties for the current file or <see langword="null" /> if no summary properties
        /// are defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.SummaryProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual SummaryPropertySet SummaryProperties { get => _summaryProperties.Entity; set => _summaryProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated document properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="DocumentPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no document properties.</value>
        public override Guid? DocumentPropertySetId { get => _documentProperties.IdValue; set => _documentProperties.SetId(value); }

        /// <summary>
        /// Gets the document properties for the current file.
        /// </summary>
        /// <value>The <see cref="DocumentPropertySet" /> that contains the document properties for the current file or <see langword="null" /> if no document
        /// properties are defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DocumentProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DocumentPropertySet DocumentProperties { get => _documentProperties.Entity; set => _documentProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated audio properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="AudioPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no audio properties.</value>
        public override Guid? AudioPropertySetId { get => _audioProperties.IdValue; set => _audioProperties.SetId(value); }

        /// <summary>
        /// Gets the audio properties for the current file.
        /// </summary>
        /// <value>The <see cref="AudioPropertySet" /> that contains the audio properties for the current file or <see langword="null" /> if no audio properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.AudioProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual AudioPropertySet AudioProperties { get => _audioProperties.Entity; set => _audioProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated DRM properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="DRMPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no DRM properties.</value>
        public override Guid? DRMPropertySetId { get => _drmProperties.IdValue; set => _drmProperties.SetId(value); }

        /// <summary>
        /// Gets the DRM properties for the current file.
        /// </summary>
        /// <value>The <see cref="DRMPropertySet" /> that contains the DRM properties for the current file or <see langword="null" /> if no DRM properties are defined
        /// on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DRMProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DRMPropertySet DRMProperties { get => _drmProperties.Entity; set => _drmProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated GPS properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="GPSPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no GPS properties.</value>
        public override Guid? GPSPropertySetId { get => _gpsProperties.IdValue; set => _gpsProperties.SetId(value); }

        /// <summary>
        /// Gets the GPS properties for the current file.
        /// </summary>
        /// <value>The <see cref="GPSPropertySet" /> that contains the GPS properties for the current file or <see langword="null" /> if no GPS properties are defined
        /// on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.GPSProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual GPSPropertySet GPSProperties { get => _gpsProperties.Entity; set => _gpsProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated image properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="ImagePropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no image properties.</value>
        public override Guid? ImagePropertySetId { get => _imageProperties.IdValue; set => _imageProperties.SetId(value); }

        /// <summary>
        /// Gets the image properties for the current file.
        /// </summary>
        /// <value>The <see cref="ImagePropertySet" /> that contains the image properties for the current file or <see langword="null" /> if no image properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.ImageProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ImagePropertySet ImageProperties { get => _imageProperties.Entity; set => _imageProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated media properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="MediaPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no media properties.</value>
        public override Guid? MediaPropertySetId { get => _mediaProperties.IdValue; set => _mediaProperties.SetId(value); }

        /// <summary>
        /// Gets the media properties for the current file.
        /// </summary>
        /// <value>The <see cref="MediaPropertySet" /> that contains the media properties for the current file or <see langword="null" /> if no media properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.MediaProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MediaPropertySet MediaProperties { get => _mediaProperties.Entity; set => _mediaProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated music properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="MusicPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no music properties.</value>
        public override Guid? MusicPropertySetId { get => _musicProperties.IdValue; set => _musicProperties.SetId(value); }

        /// <summary>
        /// Gets the music properties for the current file.
        /// </summary>
        /// <value>The <see cref="MusicPropertySet" /> that contains the music properties for the current file or <see langword="null" /> if no music properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.MusicProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual MusicPropertySet MusicProperties { get => _musicProperties.Entity; set => _musicProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated photo properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="PhotoPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no photo properties.</value>
        public override Guid? PhotoPropertySetId { get => _photoProperties.IdValue; set => _photoProperties.SetId(value); }

        /// <summary>
        /// Gets the photo properties for the current file.
        /// </summary>
        /// <value>The <see cref="PhotoPropertySet" /> that contains the photo properties for the current file or <see langword="null" /> if no photo properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.PhotoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual PhotoPropertySet PhotoProperties { get => _photoProperties.Entity; set => _photoProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated recorded TV properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="RecordedTVPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no recorded TV properties.</value>
        public override Guid? RecordedTVPropertySetId { get => _recordedTVProperties.IdValue; set => _recordedTVProperties.SetId(value); }

        /// <summary>
        /// Gets the recorded tv properties for the current file.
        /// </summary>
        /// <value>The <see cref="RecordedTVPropertySet" /> that contains the recorded TV properties for the current file or <see langword="null" /> if no recorded
        /// TV properties are defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.RecordedTVProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual RecordedTVPropertySet RecordedTVProperties { get => _recordedTVProperties.Entity; set => _recordedTVProperties.Entity = value; }

        /// <summary>
        /// Gets unique identifier of the associated video properties entity.
        /// </summary>
        /// <value>The <see cref="PropertiesRow.Id" /> of the <see cref="VideoPropertySet" /> for the current file or <see langword="null" /> if
        /// the current file has no video properties.</value>
        public override Guid? VideoPropertySetId { get => _videoProperties.IdValue; set => _videoProperties.SetId(value); }

        /// <summary>
        /// Gets the video properties for the current file.
        /// </summary>
        /// <value>The <see cref="VideoPropertySet" /> that contains the video properties for the current file or <see langword="null" /> if no video properties are
        /// defined on the current file.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.VideoProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual VideoPropertySet VideoProperties { get => _videoProperties.Entity; set => _videoProperties.Entity = value; }

        /// <summary>
        /// Gets the redundancy item that indicates the membership of a collection of redundant files.
        /// </summary>
        /// <value>
        /// An <see cref="ILocalRedundancy" /> object that indicates the current file is an exact copy of other files that belong to the
        /// same <see cref="IRedundancy.RedundantSet" />
        /// or <see langword="null" /> if this file has not been identified as being redundant with any other.
        /// </value>
        public virtual Redundancy Redundancy { get; set; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [BackingField(nameof(_accessErrors))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        public virtual HashSet<FileAccessError> AccessErrors { get => _accessErrors; set => _accessErrors = value ?? []; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IComparison.Baseline" />.
        /// </summary>
        /// <value>The <see cref="ILocalComparison" /> entities where the current file is the <see cref="IComparison.Baseline" />.</value>
        [BackingField(nameof(_baselineComparisons))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.BaselineComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        public virtual HashSet<FileComparison> BaselineComparisons { get => _baselineComparisons; set => _baselineComparisons = value ?? []; }

        /// <summary>
        /// Gets the comparisons where the current file was the <see cref="IComparison.Correlative" /> being compared to a separate <see cref="IComparison.Baseline" /> file.
        /// </summary>
        /// <value>The <see cref="ILocalComparison" /> entities where the current file is the <see cref="IComparison.Correlative" />.</value>
        [BackingField(nameof(_correlativeComparisons))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.CorrelativeComparisons), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        public virtual HashSet<FileComparison> CorrelativeComparisons { get => _correlativeComparisons; set => _correlativeComparisons = value ?? []; }

        /// <summary>
        /// Gets the personal tags associated with the current file.
        /// </summary>
        /// <value>The <see cref="ILocalPersonalFileTag"/> entities that associate <see cref="ILocalPersonalTagDefinition"/> entities with the current file.</value>
        [BackingField(nameof(_personalTags))]
        [NotNull]
        public HashSet<PersonalFileTag> PersonalTags { get => _personalTags; set => _personalTags = value ?? []; }

        /// <summary>
        /// Gets the shared tags associated with the current file.
        /// </summary>
        /// <value>The <see cref="ILocalSharedFileTag"/> entities that associate <see cref="ILocalSharedTagDefinition"/> entities with the current file.</value>
        [BackingField(nameof(_sharedTags))]
        [NotNull]
        public HashSet<SharedFileTag> SharedTags { get => _sharedTags; set => _sharedTags = value ?? []; }

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

        #endregion

        /// <summary>
        /// Creates a new file entity.
        /// </summary>
        public DbFile()
        {
            _parent = new(SyncRoot);
            _binaryProperties = new(SyncRoot);
            _summaryProperties = new(SyncRoot);
            _documentProperties = new(SyncRoot);
            _audioProperties = new(SyncRoot);
            _drmProperties = new(SyncRoot);
            _gpsProperties = new(SyncRoot);
            _imageProperties = new(SyncRoot);
            _mediaProperties = new(SyncRoot);
            _musicProperties = new(SyncRoot);
            _photoProperties = new(SyncRoot);
            _recordedTVProperties = new(SyncRoot);
            _videoProperties = new(SyncRoot);
        }

        /// <summary>
        /// Asynchronously deletes the specified <see cref="DbFile" /> entity and all descendant entities.
        /// </summary>
        /// <param name="target">The <see cref="DbFile" /> entity to delete.</param>
        /// <param name="dbContext">The database connection context.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation</param>
        /// <param name="deletionOption">Controls whether the entity is deleted or it is simply marked as a deleted file.</param>
        /// <returns><see langword="true" /> if successful; otherwise, <see langword="false" />.</returns>
        public static async Task<bool> DeleteAsync([DisallowNull] DbFile target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] CancellationToken cancellationToken, ItemDeletionOption deletionOption = ItemDeletionOption.Default)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
            EntityEntry<DbFile> entry = dbContext.Entry(target);
            if (!entry.ExistsInDb()) return false;
            bool shouldDelete;
            switch (deletionOption)
            {
                case ItemDeletionOption.Default:
                    if (target.Options.HasFlag(FileCrawlOptions.FlaggedForDeletion))
                    {
                        if (target.Status == FileCorrelationStatus.Deleted) return false;
                        target.Status = FileCorrelationStatus.Deleted;
                        shouldDelete = false;
                    }
                    else
                        shouldDelete = true;
                    break;
                case ItemDeletionOption.MarkAsDeleted:
                    if (target.Status == FileCorrelationStatus.Deleted) return false;
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
                if (item is null || oldSummaryProperties.Entity.Files.Count > 1) oldSummaryProperties = null;
            }
            EntityEntry<DocumentPropertySet> oldDocumentProperties = await entry.GetRelatedTargetEntryAsync(e => e.DocumentProperties, cancellationToken);
            if (oldDocumentProperties is not null)
            {
                DbFile item = (await oldDocumentProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldDocumentProperties.Entity.Files.Count > 1) oldDocumentProperties = null;
            }
            EntityEntry<AudioPropertySet> oldAudioProperties = await entry.GetRelatedTargetEntryAsync(e => e.AudioProperties, cancellationToken);
            if (oldAudioProperties is not null)
            {
                DbFile item = (await oldAudioProperties.GetRelatedCollectionAsync(p => p.Files, cancellationToken)).FirstOrDefault(f => f.Id == id);
                if (item is null || oldAudioProperties.Entity.Files.Count > 1) oldAudioProperties = null;
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

        /// <summary>
        /// This gets called whenever the current entity is being validated.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="results">Contains validation results to be returned by the <see cref="DbEntity.Validate(ValidationContext)"/> method.</param>
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

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="DbFile" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(DbFile other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ILocalFile" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ILocalFile other)
        {
            if (other is null) return false;
            if (other is DbFile dbFile) return Equals(dbFile);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="IFile" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(IFile other)
        {
            if (other is null) return false;
            if (other is DbFile dbFile) return Equals(dbFile);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFile local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Attempts to get the primary key of the binary properties entity.
        /// </summary>
        /// <param name="binaryPropertySetId">The <see cref="BinaryPropertySet.Id"/> value of the associated <see cref="BinaryPropertySet"/>r.</param>
        /// <returns><see langword="true"/> if <see cref="BinaryPropertySetId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        public bool TryGetBinaryPropertySetId(out Guid binaryPropertySetId) => _binaryProperties.TryGetId(out binaryPropertySetId);

        /// <summary>
        /// Attempts to get the primary key of the parent subdirectory.
        /// </summary>
        /// <param name="subdirectoryId">The <see cref="SubdirectoryRow.Id"/> value of the parent <see cref="Subdirectory"/>.</param>
        /// <returns><see langword="true"/> if the current file system item has a parent <see cref="Subdirectory"/>; otherwise, <see langword="false"/>.</returns>
        public bool TryGetParentId(out Guid subdirectoryId) => _parent.TryGetId(out subdirectoryId);

        protected class BinaryPropertySetReference : ForeignKeyReference<BinaryPropertySet>, IForeignKeyReference<ILocalBinaryPropertySet>, IForeignKeyReference<IBinaryPropertySet>
        {
            internal BinaryPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalBinaryPropertySet IForeignKeyReference<ILocalBinaryPropertySet>.Entity => Entity;

            IBinaryPropertySet IForeignKeyReference<IBinaryPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalBinaryPropertySet>>.Equals(IForeignKeyReference<ILocalBinaryPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<IBinaryPropertySet>>.Equals(IForeignKeyReference<IBinaryPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class SummaryPropertySetReference : ForeignKeyReference<SummaryPropertySet>, IForeignKeyReference<ILocalSummaryPropertySet>, IForeignKeyReference<ISummaryPropertySet>
        {
            internal SummaryPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalSummaryPropertySet IForeignKeyReference<ILocalSummaryPropertySet>.Entity => Entity;

            ISummaryPropertySet IForeignKeyReference<ISummaryPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalSummaryPropertySet>>.Equals(IForeignKeyReference<ILocalSummaryPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ISummaryPropertySet>>.Equals(IForeignKeyReference<ISummaryPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class DocumentPropertySetReference : ForeignKeyReference<DocumentPropertySet>, IForeignKeyReference<ILocalDocumentPropertySet>, IForeignKeyReference<IDocumentPropertySet>
        {
            internal DocumentPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalDocumentPropertySet IForeignKeyReference<ILocalDocumentPropertySet>.Entity => Entity;

            IDocumentPropertySet IForeignKeyReference<IDocumentPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IDocumentPropertySet>>.Equals(IForeignKeyReference<IDocumentPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalDocumentPropertySet>>.Equals(IForeignKeyReference<ILocalDocumentPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class AudioPropertySetReference : ForeignKeyReference<AudioPropertySet>, IForeignKeyReference<ILocalAudioPropertySet>, IForeignKeyReference<IAudioPropertySet>
        {
            internal AudioPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalAudioPropertySet IForeignKeyReference<ILocalAudioPropertySet>.Entity => Entity;

            IAudioPropertySet IForeignKeyReference<IAudioPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IAudioPropertySet>>.Equals(IForeignKeyReference<IAudioPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalAudioPropertySet>>.Equals(IForeignKeyReference<ILocalAudioPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class DRMPropertySetReference : ForeignKeyReference<DRMPropertySet>, IForeignKeyReference<ILocalDRMPropertySet>, IForeignKeyReference<IDRMPropertySet>
        {
            internal DRMPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalDRMPropertySet IForeignKeyReference<ILocalDRMPropertySet>.Entity => Entity;

            IDRMPropertySet IForeignKeyReference<IDRMPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ILocalDRMPropertySet>>.Equals(IForeignKeyReference<ILocalDRMPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<IDRMPropertySet>>.Equals(IForeignKeyReference<IDRMPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class GPSPropertySetReference : ForeignKeyReference<GPSPropertySet>, IForeignKeyReference<ILocalGPSPropertySet>, IForeignKeyReference<IGPSPropertySet>
        {
            internal GPSPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalGPSPropertySet IForeignKeyReference<ILocalGPSPropertySet>.Entity => Entity;

            IGPSPropertySet IForeignKeyReference<IGPSPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IGPSPropertySet>>.Equals(IForeignKeyReference<IGPSPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalGPSPropertySet>>.Equals(IForeignKeyReference<ILocalGPSPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class ImagePropertySetReference : ForeignKeyReference<ImagePropertySet>, IForeignKeyReference<ILocalImagePropertySet>, IForeignKeyReference<IImagePropertySet>
        {
            internal ImagePropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalImagePropertySet IForeignKeyReference<ILocalImagePropertySet>.Entity => Entity;

            IImagePropertySet IForeignKeyReference<IImagePropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IImagePropertySet>>.Equals(IForeignKeyReference<IImagePropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalImagePropertySet>>.Equals(IForeignKeyReference<ILocalImagePropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class MediaPropertySetReference : ForeignKeyReference<MediaPropertySet>, IForeignKeyReference<ILocalMediaPropertySet>, IForeignKeyReference<IMediaPropertySet>
        {
            internal MediaPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalMediaPropertySet IForeignKeyReference<ILocalMediaPropertySet>.Entity => Entity;

            IMediaPropertySet IForeignKeyReference<IMediaPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IMediaPropertySet>>.Equals(IForeignKeyReference<IMediaPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalMediaPropertySet>>.Equals(IForeignKeyReference<ILocalMediaPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class MusicPropertySetReference : ForeignKeyReference<MusicPropertySet>, IForeignKeyReference<ILocalMusicPropertySet>, IForeignKeyReference<IMusicPropertySet>
        {
            internal MusicPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalMusicPropertySet IForeignKeyReference<ILocalMusicPropertySet>.Entity => Entity;

            IMusicPropertySet IForeignKeyReference<IMusicPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IMusicPropertySet>>.Equals(IForeignKeyReference<IMusicPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalMusicPropertySet>>.Equals(IForeignKeyReference<ILocalMusicPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class PhotoPropertySetReference : ForeignKeyReference<PhotoPropertySet>, IForeignKeyReference<ILocalPhotoPropertySet>, IForeignKeyReference<IPhotoPropertySet>
        {
            internal PhotoPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalPhotoPropertySet IForeignKeyReference<ILocalPhotoPropertySet>.Entity => Entity;

            IPhotoPropertySet IForeignKeyReference<IPhotoPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IPhotoPropertySet>>.Equals(IForeignKeyReference<IPhotoPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalPhotoPropertySet>>.Equals(IForeignKeyReference<ILocalPhotoPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class RecordedTVPropertySetReference : ForeignKeyReference<RecordedTVPropertySet>, IForeignKeyReference<ILocalRecordedTVPropertySet>, IForeignKeyReference<IRecordedTVPropertySet>
        {
            internal RecordedTVPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalRecordedTVPropertySet IForeignKeyReference<ILocalRecordedTVPropertySet>.Entity => Entity;

            IRecordedTVPropertySet IForeignKeyReference<IRecordedTVPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IRecordedTVPropertySet>>.Equals(IForeignKeyReference<IRecordedTVPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalRecordedTVPropertySet>>.Equals(IForeignKeyReference<ILocalRecordedTVPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class VideoPropertySetReference : ForeignKeyReference<VideoPropertySet>, IForeignKeyReference<ILocalVideoPropertySet>, IForeignKeyReference<IVideoPropertySet>
        {
            internal VideoPropertySetReference(object syncRoot) : base(syncRoot) { }

            ILocalVideoPropertySet IForeignKeyReference<ILocalVideoPropertySet>.Entity => Entity;

            IVideoPropertySet IForeignKeyReference<IVideoPropertySet>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<IVideoPropertySet>>.Equals(IForeignKeyReference<IVideoPropertySet> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalVideoPropertySet>>.Equals(IForeignKeyReference<ILocalVideoPropertySet> other)
            {
                throw new NotImplementedException();
            }
        }

        protected class SubdirectoryReference : ForeignKeyReference<Subdirectory>, IForeignKeyReference<ILocalSubdirectory>, IForeignKeyReference<ISubdirectory>
        {
            internal SubdirectoryReference(object syncRoot) : base(syncRoot) { }

            ILocalSubdirectory IForeignKeyReference<ILocalSubdirectory>.Entity => Entity;

            ISubdirectory IForeignKeyReference<ISubdirectory>.Entity => Entity;

            bool IEquatable<IForeignKeyReference<ISubdirectory>>.Equals(IForeignKeyReference<ISubdirectory> other)
            {
                throw new NotImplementedException();
            }

            bool IEquatable<IForeignKeyReference<ILocalSubdirectory>>.Equals(IForeignKeyReference<ILocalSubdirectory> other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
