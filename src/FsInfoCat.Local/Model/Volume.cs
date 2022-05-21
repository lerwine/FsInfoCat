using FsInfoCat.Model;
using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Database entity which represents a logical file system volume on the local host machine.
    /// </summary>
    /// <seealso cref="VolumeRow" />
    /// <seealso cref="ILocalVolume" />
    /// <seealso cref="IEquatable{T}" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Volume : VolumeRow, ILocalVolume, IEquatable<Volume>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        #region Fields

        [Obsolete("Replace with ForeignKeyReference<FileSystem>")]
        private Guid? _fileSystemId;
        private FileSystem _fileSystem;
        private HashSet<VolumeAccessError> _accessErrors = new();
        private HashSet<PersonalVolumeTag> _personalTags = new();
        private HashSet<SharedVolumeTag> _sharedTags = new();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique identifier of the entity host file system.
        /// </summary>
        /// <value>The unique identifier of the entity that represents the host file system for the current volume.</value>
        public override Guid FileSystemId
        {
            get => _fileSystem?.Id ?? _fileSystemId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_fileSystem is not null)
                    {
                        if (_fileSystem.Id.Equals(value)) return;
                        _fileSystem = null;
                    }
                    _fileSystemId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets the file system type.
        /// </summary>
        /// <value>The file system type for this volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _fileSystem is not null && ReferenceEquals(value, _fileSystem)) return;
                    _fileSystemId = null;
                    _fileSystem = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets the root directory of this volume.
        /// </summary>
        /// <value>The root directory of this volume.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Subdirectory RootDirectory { get; set; }

        /// <summary>
        /// Gets the access errors for the current file system item.
        /// </summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [NotNull]
        [BackingField(nameof(_accessErrors))]
        public virtual HashSet<VolumeAccessError> AccessErrors { get => _accessErrors; set => _accessErrors = value ?? new(); }

        /// <summary>
        /// Gets the personal tags associated with the current volume.
        /// </summary>
        /// <value>The <see cref="PersonalVolumeTag"/> entities that associate <see cref="PersonalTagDefinition"/> entities with the current volume.</value>
        [NotNull]
        [BackingField(nameof(_personalTags))]
        public HashSet<PersonalVolumeTag> PersonalTags { get => _personalTags; set => _personalTags = value ?? new(); }

        /// <summary>
        /// Gets the shared tags associated with the current volume.
        /// </summary>
        /// <value>The <see cref="SharedVolumeTag"/> entities that associate <see cref="SharedTagDefinition"/> entities with the current volume.</value>
        [NotNull]
        [BackingField(nameof(_sharedTags))]
        public HashSet<SharedVolumeTag> SharedTags { get => _sharedTags; set => _sharedTags = value ?? new(); }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalVolume.FileSystem { get => FileSystem; }

        IFileSystem IVolume.FileSystem { get => FileSystem; }

        ILocalSubdirectory ILocalVolume.RootDirectory => RootDirectory;

        ISubdirectory IVolume.RootDirectory => RootDirectory;

        IEnumerable<ILocalVolumeAccessError> ILocalVolume.AccessErrors => AccessErrors.Cast<ILocalVolumeAccessError>();

        IEnumerable<IVolumeAccessError> IVolume.AccessErrors => AccessErrors.Cast<IVolumeAccessError>();

        IEnumerable<ILocalPersonalVolumeTag> ILocalVolume.PersonalTags => PersonalTags.Cast<ILocalPersonalVolumeTag>();

        IEnumerable<IPersonalVolumeTag> IVolume.PersonalTags => PersonalTags.Cast<IPersonalVolumeTag>();

        IEnumerable<ILocalSharedVolumeTag> ILocalVolume.SharedTags => SharedTags.Cast<ILocalSharedVolumeTag>();

        IEnumerable<ISharedVolumeTag> IVolume.SharedTags => SharedTags.Cast<ISharedVolumeTag>();

        #endregion

        [Obsolete("Use FsInfoCat.Local.Background.IDeleteVolumeBackgroundService")]
        internal async Task<bool> ForceDeleteFromDbAsync(LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            EntityEntry<Volume> dbEntry = dbContext.Entry(this);
            if (!dbEntry.ExistsInDb())
                return false;
            cancellationToken.ThrowIfCancellationRequested();
            IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            Subdirectory oldSubdirectory = await dbEntry.GetRelatedReferenceAsync(f => f.RootDirectory, cancellationToken);
            if (oldSubdirectory is not null)
                await Subdirectory.DeleteAsync(oldSubdirectory, dbContext, cancellationToken, ItemDeletionOption.Force);
            cancellationToken.ThrowIfCancellationRequested();
            VolumeAccessError[] accessErrors = (await dbEntry.GetRelatedCollectionAsync(f => f.AccessErrors, cancellationToken)).ToArray();
            if (accessErrors.Length > 0)
                dbContext.VolumeAccessErrors.RemoveRange(accessErrors);
            cancellationToken.ThrowIfCancellationRequested();
            _ = await dbContext.SaveChangesAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            await transaction.CommitAsync(cancellationToken);
            return true;
        }

        internal static void OnBuildEntity(EntityTypeBuilder<Volume> builder)
        {
            _ = builder.HasOne(sn => sn.FileSystem).WithMany(d => d.Volumes).HasForeignKey(nameof(FileSystemId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            _ = builder.Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);
        }

        [Obsolete("Use method with IFileSystemDetailService")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static async Task<EntityEntry<Volume>> ImportVolumeAsync([DisallowNull] DirectoryInfo directoryInfo, [DisallowNull] LocalDbContext dbContext,
            CancellationToken cancellationToken)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            if (directoryInfo.Parent is not null)
                directoryInfo = directoryInfo.Root;

            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            IFileSystemDetailService fileSystemDetailService = serviceScope.ServiceProvider.GetService<IFileSystemDetailService>();
            //string name = directoryInfo.Name;
            ILogicalDiskInfo diskInfo = await fileSystemDetailService.GetLogicalDiskAsync(directoryInfo, cancellationToken);
            VolumeIdentifier volumeIdentifier;
            if (diskInfo is null)
            {
                Uri uri = new(((directoryInfo.Parent is null) ? directoryInfo : directoryInfo.Root).FullName, UriKind.Absolute);
                if (uri.IsUnc)
                    volumeIdentifier = new VolumeIdentifier(uri);
                else
                    throw new InvalidOperationException($"Logical disk \"{directoryInfo.FullName}\" not found.");
            }
            else if (!diskInfo.TryGetVolumeIdentifier(out volumeIdentifier))
                throw new InvalidOperationException($"Logical disk \"{diskInfo.Name}\" does not specify a volume identifer.");

            Volume result = await (from v in dbContext.Volumes where v.Identifier == volumeIdentifier select v).FirstOrDefaultAsync(cancellationToken);
            if (result is not null)
                return dbContext.Entry(result);
            (EntityEntry<FileSystem> Entry, SymbolicName SymbolicName) fileSystem = await FileSystem.ImportFileSystemAsync(diskInfo, volumeIdentifier, dbContext,
                fileSystemDetailService, cancellationToken);
            result = new()
            {
                Id = Guid.NewGuid(),
                Identifier = volumeIdentifier,
                FileSystem = fileSystem.Entry.Entity
            };
            if (diskInfo is null)
            {
                (IFileSystemProperties Properties, string SymbolicName) genericNetworkFsType = fileSystemDetailService.GetGenericNetworkShareFileSystem();
                result.MaxNameLength = genericNetworkFsType.Properties.MaxNameLength;
                result.ReadOnly = genericNetworkFsType.Properties.ReadOnly;
                result.Status = VolumeStatus.Unknown;
                result.Type = DriveType.Network;
                result.DisplayName = $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}";
            }
            else
            {
                result.MaxNameLength = diskInfo.MaxNameLength;
                result.ReadOnly = diskInfo.IsReadOnly;
                result.Status = VolumeStatus.Unknown;
                result.Type = diskInfo.DriveType;
                result.DisplayName = (diskInfo.DriveType == DriveType.Network && diskInfo.DisplayName == directoryInfo.FullName) ?
                    $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}" : diskInfo.DisplayName;
            }
            if (fileSystem.Entry.State == EntityState.Added)
            {
                result.ModifiedOn = result.CreatedOn = fileSystem.Entry.Entity.CreatedOn;
                _ = await dbContext.SaveChangesAsync(cancellationToken);
                _ = dbContext.SymbolicNames.Add(fileSystem.SymbolicName);
                _ = await dbContext.SaveChangesAsync(cancellationToken);
            }
            else
                result.ModifiedOn = result.CreatedOn = DateTime.Now;
            return dbContext.Volumes.Add(result);
        }

        /// <summary>
        /// Asynchronously imports a volume.
        /// </summary>
        /// <param name="directoryInfo">The root directory of the volume to import.</param>
        /// <param name="fileSystemDetailService">The <see cref="IFileSystemDetailService" /> for getting file system information from the local host.</param>
        /// <param name="dbContext">The database connection context.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken" /> that can be used to cancel the asynchronous operation</param>
        /// <returns></returns>
        public static async Task<EntityEntry<Volume>> ImportVolumeAsync([DisallowNull] DirectoryInfo directoryInfo, IFileSystemDetailService fileSystemDetailService,
            [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            if (directoryInfo.Parent is not null)
                directoryInfo = directoryInfo.Root;

            //string name = directoryInfo.Name;
            ILogicalDiskInfo diskInfo = await fileSystemDetailService.GetLogicalDiskAsync(directoryInfo, cancellationToken);
            VolumeIdentifier volumeIdentifier;
            if (diskInfo is null)
            {
                Uri uri = new(((directoryInfo.Parent is null) ? directoryInfo : directoryInfo.Root).FullName, UriKind.Absolute);
                if (uri.IsUnc)
                    volumeIdentifier = new VolumeIdentifier(uri);
                else
                    throw new InvalidOperationException($"Logical disk \"{directoryInfo.FullName}\" not found.");
            }
            else if (!diskInfo.TryGetVolumeIdentifier(out volumeIdentifier))
                throw new InvalidOperationException($"Logical disk \"{diskInfo.Name}\" does not specify a volume identifer.");

            Volume result = await (from v in dbContext.Volumes where v.Identifier == volumeIdentifier select v).FirstOrDefaultAsync(cancellationToken);
            if (result is not null)
                return dbContext.Entry(result);
            (EntityEntry<FileSystem> Entry, SymbolicName SymbolicName) fileSystem = await FileSystem.ImportFileSystemAsync(diskInfo, volumeIdentifier, dbContext,
                fileSystemDetailService, cancellationToken);
            result = new()
            {
                Id = Guid.NewGuid(),
                Identifier = volumeIdentifier,
                FileSystem = fileSystem.Entry.Entity
            };
            if (diskInfo is null)
            {
                (IFileSystemProperties Properties, string SymbolicName) genericNetworkFsType = fileSystemDetailService.GetGenericNetworkShareFileSystem();
                result.MaxNameLength = genericNetworkFsType.Properties.MaxNameLength;
                result.ReadOnly = genericNetworkFsType.Properties.ReadOnly;
                result.Status = VolumeStatus.Unknown;
                result.Type = DriveType.Network;
                result.DisplayName = $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}";
            }
            else
            {
                result.MaxNameLength = diskInfo.MaxNameLength;
                result.ReadOnly = diskInfo.IsReadOnly;
                result.Status = VolumeStatus.Unknown;
                result.Type = diskInfo.DriveType;
                result.DisplayName = (diskInfo.DriveType == DriveType.Network && diskInfo.DisplayName == directoryInfo.FullName) ?
                    $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}" : diskInfo.DisplayName;
            }
            if (fileSystem.Entry.State == EntityState.Added)
            {
                result.ModifiedOn = result.CreatedOn = fileSystem.Entry.Entity.CreatedOn;
                _ = await dbContext.SaveChangesAsync(cancellationToken);
                _ = dbContext.SymbolicNames.Add(fileSystem.SymbolicName);
                _ = await dbContext.SaveChangesAsync(cancellationToken);
            }
            else
                result.ModifiedOn = result.CreatedOn = DateTime.Now;
            return dbContext.Volumes.Add(result);
        }

        /// <summary>
        /// Asyncronously removes a volume and all related entities from the database.
        /// </summary>
        /// <param name="target">The <see cref="Volume" /> entity to delete.</param>
        /// <param name="dbContext">The database connection context.</param>
        /// <param name="progress">The <see cref="IActivityProgress" /> object that receives progress updates.</param>
        /// <param name="logger">The <see cref="ILogger" /> for event logging.</param>
        /// <returns>A <see cref="Task{T}" /> that returns the number of entities deleted.</returns>
        public static async Task<int> DeleteAsync([DisallowNull] Volume target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IActivityProgress progress, ILogger logger)
        {
            if (target is null) throw new ArgumentNullException(nameof(target));
            if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
            if (progress is null) throw new ArgumentNullException(nameof(progress));
            if (logger is null) throw new ArgumentNullException(nameof(logger));
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            using (logger.BeginScope(target.Id))
            {
                progress.Report($"Removing volume record: {target.Identifier}");
                EntityEntry<Volume> entry = dbContext.Entry(target);
                logger.LogDebug("Removing dependant records for Subdirectory {{ Id = {Id}; Identifier = {Identifier} }}", target.Id, target.Identifier);
                Subdirectory subdirectory = await entry.GetRelatedReferenceAsync(e => e.RootDirectory, progress.Token);
                int result = (subdirectory is null || !(await Subdirectory.DeleteAsync(subdirectory, dbContext, progress.Token, ItemDeletionOption.Force))) ? 0 : 1;
                PersonalVolumeTag[] pvt = (await entry.GetRelatedCollectionAsync(e => e.PersonalTags, progress.Token)).ToArray();
                if (pvt.Length > 0)
                    dbContext.PersonalVolumeTags.RemoveRange(pvt);
                SharedVolumeTag[] svt = (await entry.GetRelatedCollectionAsync(e => e.SharedTags, progress.Token)).ToArray();
                if (svt.Length > 0)
                    dbContext.SharedVolumeTags.RemoveRange(svt);
                VolumeAccessError[] ve = (await entry.GetRelatedCollectionAsync(e => e.AccessErrors, progress.Token)).ToArray();
                if (ve.Length > 0)
                    dbContext.VolumeAccessErrors.RemoveRange(ve);
                if (dbContext.ChangeTracker.HasChanges())
                    result += await dbContext.SaveChangesAsync(progress.Token);
                logger.LogInformation("Removing Volume {{ Id = {Id}; Identifier = {Identifier} }}", target.Id, target.Identifier);
                _ = dbContext.Volumes.Remove(target);
                result += await dbContext.SaveChangesAsync(progress.Token);
                await transaction.CommitAsync();
                return result;
            }
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="Volume" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(Volume other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ILocalVolume" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ILocalVolume other)
        {
            if (other is null) return false;
            if (other is Volume volume) return Equals(volume);
            if (TryGetId(out Guid id1)) return other.TryGetId(out Guid id2) && id1.Equals(id2);
            return !other.TryGetId(out _) && ArePropertiesEqual(other);
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="IVolume" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(IVolume other)
        {
            if (other is null) return false;
            if (other is Volume volume) return Equals(volume);
            if (TryGetId(out Guid id1)) return other.TryGetId(out Guid id2) && id1.Equals(id2);
            return !other.TryGetId(out _) && ((other is ILocalVolume local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Equals(object obj)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (obj is null) return false;
            if (obj is Volume volume) return Equals(volume);
            return obj is IVolumeRow row && (TryGetId(out Guid id1) ? row.TryGetId(out Guid id2) && id1.Equals(id2) :
                (!row.TryGetId(out _) && ((row is ILocalVolumeRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(row))));
        }

        /// <summary>
        /// Attempts to get the primary key of the associated filesystem.
        /// </summary>
        /// <param name="fileSystemId">The <see cref="IHasSimpleIdentifier.Id"/> of the associated <see cref="IFileSystem"/>.</param>
        /// <returns><see langword="true"/> if <see cref="FileSystemId"/> has a foreign key value assigned; otherwise, <see langword="false"/>.</returns>
        public bool TryGetFileSystemId(out Guid fileSystemId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_fileSystem is null)
                {
                    if (_fileSystemId.HasValue)
                    {
                        fileSystemId = _fileSystemId.Value;
                        return true;
                    }
                }
                else
                    return _fileSystem.TryGetId(out fileSystemId);
            }
            finally { Monitor.Exit(SyncRoot); }
            fileSystemId = Guid.Empty;
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
