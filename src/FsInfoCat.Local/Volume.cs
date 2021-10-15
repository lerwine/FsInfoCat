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

namespace FsInfoCat.Local
{
    public class Volume : VolumeRow, ILocalVolume, ISimpleIdentityReference<Volume>
    {
        #region Fields

        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;
        private readonly IPropertyChangeTracker<Subdirectory> _rootDirectory;
        private HashSet<VolumeAccessError> _accessErrors = new();
        private HashSet<PersonalVolumeTag> _personalTags = new();
        private HashSet<SharedVolumeTag> _sharedTags = new();

        #endregion

        #region Properties

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem.GetValue();
            set
            {
                if (_fileSystem.SetValue(value))
                    FileSystemId = value?.Id ?? Guid.Empty;
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Subdirectory RootDirectory { get => _rootDirectory.GetValue(); set => _rootDirectory.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<VolumeAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
        }

        public HashSet<PersonalVolumeTag> PersonalTags
        {
            get => _personalTags;
            set => CheckHashSetChanged(_personalTags, value, h => _personalTags = h);
        }

        public HashSet<SharedVolumeTag> SharedTags
        {
            get => _sharedTags;
            set => CheckHashSetChanged(_sharedTags, value, h => _sharedTags = h);
        }

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

        Volume IIdentityReference<Volume>.Entity => this;

        #endregion

        public Volume()
        {
            _fileSystem = AddChangeTracker<FileSystem>(nameof(FileSystem), null);
            _rootDirectory = AddChangeTracker<Subdirectory>(nameof(RootDirectory), null);
        }

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
            Guid id = Id;
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

        public static async Task<EntityEntry<Volume>> ImportVolumeAsync([DisallowNull] DirectoryInfo directoryInfo, [DisallowNull] LocalDbContext dbContext, CancellationToken cancellationToken)
        {
            if (directoryInfo is null)
                throw new ArgumentNullException(nameof(directoryInfo));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));

            if (directoryInfo.Parent is not null)
                directoryInfo = directoryInfo.Root;

            using IServiceScope serviceScope = Services.ServiceProvider.CreateScope();
            IFileSystemDetailService fileSystemDetailService = serviceScope.ServiceProvider.GetService<IFileSystemDetailService>();
            string name = directoryInfo.Name;
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
            (EntityEntry<FileSystem> Entry, SymbolicName SymbolicName) fileSystem = await FileSystem.ImportFileSystemAsync(diskInfo, volumeIdentifier, dbContext, fileSystemDetailService, cancellationToken);
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
                result.DisplayName = (diskInfo.DriveType == DriveType.Network && diskInfo.DisplayName == directoryInfo.FullName) ? $"{volumeIdentifier.Location.PathAndQuery[1..]} on {volumeIdentifier.Location.Host}" : diskInfo.DisplayName;
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

        [Obsolete("Use FsInfoCat.Local.Background.IDeleteVolumeBackgroundService")]
        public static async Task<int> DeleteAsync([DisallowNull] Volume target, [DisallowNull] LocalDbContext dbContext, [DisallowNull] IStatusListener statusListener)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (dbContext is null)
                throw new ArgumentNullException(nameof(dbContext));
            if (statusListener is null)
                throw new ArgumentNullException(nameof(statusListener));
            using IDbContextTransaction transaction = dbContext.Database.BeginTransaction();
            using IDisposable loggerScope = statusListener.Logger.BeginScope(target.Id);
            statusListener.SetMessage($"Removing volume record: {target.Identifier}");
            EntityEntry<Volume> entry = dbContext.Entry(target);
            statusListener.Logger.LogDebug("Removing dependant records for Subdirectory {{ Id = {Id}; Identifier = {Identifier} }}", target.Id, target.Identifier);
            Subdirectory subdirectory = await entry.GetRelatedReferenceAsync(e => e.RootDirectory, statusListener.CancellationToken);
            int result = (subdirectory is null || !(await Subdirectory.DeleteAsync(subdirectory, dbContext, statusListener.CancellationToken, ItemDeletionOption.Force))) ? 0 : 1;
            PersonalVolumeTag[] pvt = (await entry.GetRelatedCollectionAsync(e => e.PersonalTags, statusListener.CancellationToken)).ToArray();
            if (pvt.Length > 0)
                dbContext.PersonalVolumeTags.RemoveRange(pvt);
            SharedVolumeTag[] svt = (await entry.GetRelatedCollectionAsync(e => e.SharedTags, statusListener.CancellationToken)).ToArray();
            if (svt.Length > 0)
                dbContext.SharedVolumeTags.RemoveRange(svt);
            VolumeAccessError[] ve = (await entry.GetRelatedCollectionAsync(e => e.AccessErrors, statusListener.CancellationToken)).ToArray();
            if (ve.Length > 0)
                dbContext.VolumeAccessErrors.RemoveRange(ve);
            if (dbContext.ChangeTracker.HasChanges())
                result += await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            statusListener.Logger.LogInformation("Removing Volume {{ Id = {Id}; Identifier = {Identifier} }}", target.Id, target.Identifier);
            _ = dbContext.Volumes.Remove(target);
            result += await dbContext.SaveChangesAsync(statusListener.CancellationToken);
            await transaction.CommitAsync();
            return result;
        }

        protected override void OnFileSystemIdChanged(Guid value)
        {
            FileSystem nav = _fileSystem.GetValue();
            if (!(nav is null || nav.Id.Equals(value)))
                _ = _fileSystem.SetValue(null);
        }
    }
}
